/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Exceptions;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;
using AutoMapper;
using za.co.grindrodbank.a3s.A3SApiResources;
using static za.co.grindrodbank.a3s.Models.TransientStateMachineRecord;

namespace za.co.grindrodbank.a3s.Services
{
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionRepository functionRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IFunctionTransientRepository functionTransientRepository;
        private readonly ISubRealmRepository subRealmRepository;
        private readonly IFunctionPermissionTransientRepository functionPermissionTransientRepository;
        private readonly IMapper mapper;

        public FunctionService(IFunctionRepository functionRepository, IPermissionRepository permissionRepository, IApplicationRepository applicationRepository, IFunctionTransientRepository functionTransientRepository, ISubRealmRepository subRealmRepository, IFunctionPermissionTransientRepository functionPermissionTransientRepository, IMapper mapper)
        {
            this.functionRepository = functionRepository;
            this.applicationRepository = applicationRepository;
            this.permissionRepository = permissionRepository;
            this.functionTransientRepository = functionTransientRepository;
            this.subRealmRepository = subRealmRepository;
            this.functionPermissionTransientRepository = functionPermissionTransientRepository;

            this.mapper = mapper;
        }

        public async Task<FunctionTransient> CreateAsync(FunctionSubmit functionSubmit, Guid createdByGuid)
        {
            // Start transactions to allow complete rollback in case of an error
            InitSharedTransaction();

            try
            {
                await CheckFunctionsAndTransientFunctionsForUniqueName(functionSubmit.Name, Guid.Empty);

                FunctionTransientModel newFunctionTransient = await CaptureTransientFunctionAsync(Guid.Empty, functionSubmit.Name, functionSubmit.Description, functionSubmit.ApplicationId, functionSubmit.SubRealmId, TransientAction.Create, createdByGuid);

                // Even though we are creating/capturing the function here, it is possible that the configured approval count is 0,
                // which means that we need to check for whether the transient state is released, and process the affected function accrodingly.
                // NOTE: It is possible for an empty function (not persisted) to be returned if the function is not released in the following step.
                FunctionModel function = await UpdateFunctionBasedOnTransientActionIfTransientFunctionStateIsReleased(newFunctionTransient);

                newFunctionTransient.LatestTransientFunctionPermissions = await CaptureFunctionPermissionAssignmentChanges(function, newFunctionTransient.FunctionId, functionSubmit, createdByGuid, functionSubmit.SubRealmId);

                // It is possible that the assigned permisisons state has changed. Update the model, but only if it has an ID.
                if (function.Id != Guid.Empty)
                {
                    await functionRepository.UpdateAsync(function);
                }

                // All successful
                CommitTransaction();

                return mapper.Map<FunctionTransient>(newFunctionTransient);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        private async Task CheckFunctionsAndTransientFunctionsForUniqueName(string name, Guid functionId)
        {
            await CheckFunctionsForUniqueName(name, functionId);
            await CheckTransientFunctionsForUniqueName(name, functionId);
        }

        private async Task CheckFunctionsForUniqueName(string name, Guid functionId)
        {
            FunctionModel existingFunction = await functionRepository.GetByNameAsync(name);

            if (existingFunction != null)
            {
                if (functionId == Guid.Empty)
                {
                    throw new EntityStateConflictException($"Function with Name '{name}' already exist.");
                }
                else // If there is a function with that name, ensure it is for the function we are operating on.
                {
                    if (existingFunction.Id != functionId)
                    {
                        throw new EntityStateConflictException($"Function with Name '{name}' already exist.");
                    }
                }
            }
        }

        private async Task CheckTransientFunctionsForUniqueName(string Name, Guid functionId)
        {
            var allActiveFunctionTransients = await functionTransientRepository.GetLatestActiveTransientsForAllFunctionsAsync();
            // We can take the first transient result of active transients that have a certain name.
            var activeFunctionTransientWithName = allActiveFunctionTransients.Where(aft => aft.Name == Name).FirstOrDefault();

            if(activeFunctionTransientWithName != null)
            {
                if(functionId == Guid.Empty)
                {
                    throw new EntityStateConflictException($"There are active transient functions using the function name '{Name}'.");
                }
                else
                {
                    if(activeFunctionTransientWithName.FunctionId != functionId)
                    {
                        throw new EntityStateConflictException($"There are active transient functions using the function name '{Name}'.");
                    }
                }
            }
        }

        private async Task<FunctionTransientModel> CaptureTransientFunctionAsync(Guid functionId, string functionName, string functionDescription, Guid applicationId, Guid subRealmId, TransientAction action, Guid createdById)
        {
            FunctionTransientModel latestTransientFunction = null;
            List<FunctionTransientModel> transientFunctions = new List<FunctionTransientModel>();

            // Recall - there might not be a Guid for the function if we are creating it.
            if (functionId != Guid.Empty)
            {
                transientFunctions = await functionTransientRepository.GetTransientsForFunctionAsync(functionId);
                latestTransientFunction = transientFunctions.LastOrDefault();
            }

            if (subRealmId != Guid.Empty)
            {
                await CheckSubRealmIdIsValid(subRealmId);
            }

            FunctionTransientModel newTransientFunction = new FunctionTransientModel
            {
                Action = action,
                ChangedBy = createdById,
                ApprovalCount = latestTransientFunction == null ? 0 : latestTransientFunction.ApprovalCount,
                // Pending is the initial state of the state machine for all transient records.
                R_State = latestTransientFunction == null ? DatabaseRecordState.Pending : latestTransientFunction.R_State,
                Name = functionName,
                Description = functionDescription,
                SubRealmId = subRealmId,
                FunctionId = functionId == Guid.Empty ? Guid.NewGuid() : functionId
            };

            await CheckForApplicationAndAssignToFunctionTransientIfExists(newTransientFunction, applicationId);

            try
            {
                newTransientFunction.Capture(createdById.ToString());
            }
            catch (Exception e)
            {
                throw new InvalidStateTransitionException($"Cannot capture function with ID '{functionId}'. Error: {e.Message}");
            }

            var latestReleasedRecord = transientFunctions.Where(transientFunction => transientFunction.R_State == DatabaseRecordState.Released).LastOrDefault();

            // Only persist the new captured state of the role if it actually different from the latest released state.
            return IsCapturedFunctionDifferentFromLatestReleasedTransientFunctionState(latestReleasedRecord, functionName, functionDescription, subRealmId, action) ? await functionTransientRepository.CreateAsync(newTransientFunction) : latestTransientFunction;
        }

        private bool IsCapturedFunctionDifferentFromLatestReleasedTransientFunctionState(FunctionTransientModel latestReleasedTransientFunction, string currentFunctionName, string currentFunctionDescription, Guid currentFunctionSubRealmId, TransientAction action)
        {
            if (latestReleasedTransientFunction == null)
            {
                return true;
            }

            // Always capture the intent to perform a deletion.
            if (action == TransientAction.Delete)
            {
                return true;
            }

            return (latestReleasedTransientFunction.Name != currentFunctionName
                   || latestReleasedTransientFunction.Description != currentFunctionDescription
                   || latestReleasedTransientFunction.SubRealmId != currentFunctionSubRealmId);
        }

        private async Task CheckSubRealmIdIsValid(Guid subRealmId)
        {
            var subRealm = await subRealmRepository.GetByIdAsync(subRealmId, false);

            if (subRealm == null)
            {
                throw new ItemNotFoundException($"Sub-realm with ID '{subRealmId}' not found when attempting to assign it to a function.");
            }
        }

        private async Task<FunctionModel> UpdateFunctionBasedOnTransientActionIfTransientFunctionStateIsReleased(FunctionTransientModel functionTransientModel)
        {
            FunctionModel functionToUpdate = new FunctionModel();

            if (functionTransientModel.R_State != DatabaseRecordState.Released)
            {
                return functionToUpdate;
            }

            functionToUpdate = await functionRepository.GetByIdAsync(functionTransientModel.FunctionId);

            if (functionToUpdate == null && functionTransientModel.Action != TransientAction.Create)
            {
                throw new ItemNotFoundException($"Function with ID '{functionTransientModel.FunctionId}' not found when attempting to release function.");
            }

            if (functionTransientModel.Action == TransientAction.Modify)
            {
                await UpdateFunctionWithCurrentTransientState(functionToUpdate, functionTransientModel);
                return functionToUpdate;
            }

            if (functionTransientModel.Action == TransientAction.Delete)
            {
                await functionRepository.DeleteAsync(functionToUpdate);
                return functionToUpdate;
            }

            // Only attempt to re-create the role if there is no existing role.
            if (functionToUpdate == null)
            {
                return await CreateFunctionFromCurrentTransientState(functionTransientModel);
            }

            return functionToUpdate;
        }

        private async Task UpdateFunctionWithCurrentTransientState(FunctionModel functionToRelease, FunctionTransientModel transientFunction)
        {
            functionToRelease.Name = transientFunction.Name;
            functionToRelease.Description = transientFunction.Description;

            await functionRepository.UpdateAsync(functionToRelease);
        }

        private async Task<FunctionModel> CreateFunctionFromCurrentTransientState(FunctionTransientModel transientFunction)
        {
            FunctionModel functionToCreate = new FunctionModel
            {
                Name = transientFunction.Name,
                Description = transientFunction.Description,
                Id = transientFunction.FunctionId
            };

            await AssignSubRealmToFunctionFromTransientFunctionIfSubRealmNotEmpty(functionToCreate, transientFunction);

            return await functionRepository.CreateAsync(functionToCreate);
        }

        private async Task AssignSubRealmToFunctionFromTransientFunctionIfSubRealmNotEmpty(FunctionModel functionModel, FunctionTransientModel transientFunction)
        {
            if (transientFunction.SubRealmId == Guid.Empty)
            {
                return;
            }

            var subRealm = await subRealmRepository.GetByIdAsync(transientFunction.SubRealmId, false);

            functionModel.SubRealm = subRealm ?? throw new ItemNotProcessableException($"Sub-Realm with ID '{transientFunction.SubRealmId}' not found when attempting to assign it to a function with ID '{functionModel.Id}' from a transient state.");
        }

        private async Task<List<FunctionPermissionTransientModel>> CaptureFunctionPermissionAssignmentChanges(FunctionModel functionModel, Guid functionId, FunctionSubmit functionSubmit, Guid capturedBy, Guid functionSubRealmId)
        {
            await CheckIfThereAreExistingCapturedOrApprovedTransientFunctionPermissionsForRoleAndThrowExceptionIfThereAre(functionId);
            List<FunctionPermissionTransientModel> affectedFunctionPermissionTransientRecords = new List<FunctionPermissionTransientModel>();

            await DetectAndCaptureNewFunctionPermissionAssignments(functionModel, functionId, functionSubmit, capturedBy, functionSubRealmId, affectedFunctionPermissionTransientRecords);
            await DetectAndCapturePermissionsRemovedFromFunction(functionModel, functionId, functionSubmit, capturedBy, affectedFunctionPermissionTransientRecords);

            return affectedFunctionPermissionTransientRecords;
        }

        private async Task CheckIfThereAreExistingCapturedOrApprovedTransientFunctionPermissionsForRoleAndThrowExceptionIfThereAre(Guid functionId)
        {
            var allTransientFunctionPermissions = await functionPermissionTransientRepository.GetAllTransientPermissionRelationsForFunctionAsync(functionId);

            // Extract a distinct list of permission IDs from all the function permission transients records.
            var distinctPermissionIds = allTransientFunctionPermissions.Select(tfp => tfp.PermissionId).Distinct();

            // Iterate through all the distinc function IDs, find the latest transient record for each function, and process accordingly.
            foreach (var permissionId in distinctPermissionIds)
            {
                var latestTransientFunctionPermissionRecord = allTransientFunctionPermissions.Where(tfp => tfp.PermissionId == permissionId).LastOrDefault();

                if (latestTransientFunctionPermissionRecord.R_State == DatabaseRecordState.Captured || latestTransientFunctionPermissionRecord.R_State == DatabaseRecordState.Approved)
                {
                    throw new ItemNotProcessableException($"Cannot capture new state for function with ID '{functionId}' as there is a transient function permission for permission with ID '{permissionId}' in a '{latestTransientFunctionPermissionRecord.R_State}' state.");
                }
            }
        }

        private async Task<List<FunctionPermissionTransientModel>> DetectAndCaptureNewFunctionPermissionAssignments(FunctionModel functionModel, Guid functionId, FunctionSubmit functionSubmit, Guid capturedBy, Guid functionSubRealm, List<FunctionPermissionTransientModel> affectedFunctionPermissionTransientRecords)
        {
            // Recall, the role might not actually exist at this stage, so safely get access to a role function list.
            var currentReleasedFunctionPermissions = functionModel.FunctionPermissions ?? new List<FunctionPermissionModel>();

            foreach (var permissionId in functionSubmit.Permissions)
            {
                var existingRoleFunction = currentReleasedFunctionPermissions.Where(fp => fp.PermissionId == permissionId).FirstOrDefault();

                if (existingRoleFunction == null)
                {
                    var newTransientFunctionPermissionRecord = await CaptureFunctionPermissionAssignmentChange(functionId, permissionId, capturedBy, TransientAction.Create, functionSubRealm);
                    CheckForAndProcessReleasedFunctionPermissionTransientRecord(functionModel, newTransientFunctionPermissionRecord);
                    affectedFunctionPermissionTransientRecords.Add(newTransientFunctionPermissionRecord);
                }
            }

            return affectedFunctionPermissionTransientRecords;
        }

        private async Task<FunctionPermissionTransientModel> CaptureFunctionPermissionAssignmentChange(Guid functionId, Guid permissionId, Guid capturedBy, TransientAction action, Guid fuctionSubRealmId)
        {
            var permissionToAdd = await permissionRepository.GetByIdWithApplicationAsync(permissionId);

            if (permissionToAdd == null)
            {
                throw new ItemNotFoundException($"Permission with ID '{permissionId}' not found when attempting to assign it to a function.");
            }

            ConfirmSubRealmAssociation(fuctionSubRealmId, permissionToAdd);
            await ConfirmPermissionInSameApplicationAsFunction(functionId, permissionToAdd);

            var functionPermissionTransientRecords = await functionPermissionTransientRepository.GetTransientPermissionRelationsForFunctionAsync(functionId, permissionId);
            var latestFunctionPermissionTransientState = functionPermissionTransientRecords.LastOrDefault();

            var transientFunctionPermission = new FunctionPermissionTransientModel
            {
                PermissionId = permissionId,
                FunctionId = functionId,
                R_State = latestFunctionPermissionTransientState == null ? DatabaseRecordState.Pending : latestFunctionPermissionTransientState.R_State,
                ChangedBy = capturedBy,
                ApprovalCount = latestFunctionPermissionTransientState == null ? 0 : latestFunctionPermissionTransientState.ApprovalCount,
                Action = action
            };

            try // Attempt to transition the state of the transient function permission, but be prepared for a possible state transition exception.
            {
                transientFunctionPermission.Capture(capturedBy.ToString());
            }
            catch (Exception e)
            {
                throw new InvalidStateTransitionException($"Cannot capture function permission assignment change for function with ID '{functionId}', Permission with ID '{permissionId}'. State transition violation. Assignment Action: '{action}'. Error: {e.Message}");
            }

            await functionPermissionTransientRepository.CreateNewTransientStateForFunctionPermissionAsync(transientFunctionPermission);

            return transientFunctionPermission;
        }

        private void ConfirmSubRealmAssociation(Guid functionSubRealmId, PermissionModel permission)
        {
            // if there is a sub-realm associated with the function, we must ensure that the permission we are wanting to assign to it is at least assigned to that sub-realm.
            // Recall: Permissions have a many to many relationship with sub-realms.
            if (functionSubRealmId != null && functionSubRealmId != Guid.Empty)
            {
                if (!permission.SubRealmPermissions.Select(srp => srp.SubRealmId).ToList().Contains(functionSubRealmId))
                {
                    throw new ItemNotProcessableException($"Attempting to add a permission with ID '{permission.Id}' to a function within the sub-realm with ID '{functionSubRealmId}', but the permission does not exist within that sub-realm.");
                }
            }
        }

        private async Task ConfirmPermissionInSameApplicationAsFunction(Guid functionId, PermissionModel permission)
        {
            FunctionModel function = await functionRepository.GetByIdAsync(functionId);

            if(function == null)
            {
                // If the function is null, it might be because it is new and not released. Check for active function transients for the function, and the corresponding application.
                var latestCapturedFunctionTransient = await functionTransientRepository.GetLatestCapturedTransientForFunctionsAsync(functionId);

                if(latestCapturedFunctionTransient == null)
                {
                    throw new ItemNotFoundException($"Function or Function Transient with ID '{functionId}' not found when attepting to check it's associated application for function permission assignment checks.");
                }

                if(latestCapturedFunctionTransient.ApplicationId != permission.ApplicationFunctionPermissions.Select(afp => afp.ApplicationFunction.Application.Id).FirstOrDefault())
                {
                    throw new ItemNotProcessableException($"Cannot assign Permission with ID '{permission.Id}' to function with ID '{functionId}'. They are not related to the same application, and must be.");
                }
            }
            else
            {
                // All the application functions associated with a permission must resolve to the same application, so choosing any related application from the collection is fine.
                if (function.Application.Id != permission.ApplicationFunctionPermissions.Select(afp => afp.ApplicationFunction.Application.Id).FirstOrDefault())
                {
                    throw new ItemNotProcessableException($"Cannot assign Permission with ID '{permission.Id}' to function with ID '{functionId}'. They are not related to the same application, and must be.");
                }
            }
        }

        private void CheckForAndProcessReleasedFunctionPermissionTransientRecord(FunctionModel functionModel, FunctionPermissionTransientModel functionPermissionTransientModel)
        {
            if (functionPermissionTransientModel.R_State != DatabaseRecordState.Released)
            {
                return;
            }

            // It is important to check that the associated role actually exists.
            if (functionModel.Id == Guid.Empty)
            {
                throw new InvalidStateTransitionException($"Attempting to process a released transient function permission assignment update for function with ID '{functionPermissionTransientModel.FunctionId}' and permission with ID '{functionPermissionTransientModel.PermissionId}', but the function does not exist or is not released yet.");
            }

            // Ensure there is a function permissions relation.
            functionModel.FunctionPermissions ??= new List<FunctionPermissionModel>();

            if (functionPermissionTransientModel.Action == TransientAction.Create)
            {
                functionModel.FunctionPermissions.Add(new FunctionPermissionModel
                {
                    FunctionId = functionPermissionTransientModel.FunctionId,
                    PermissionId = functionPermissionTransientModel.PermissionId
                });

                return;
            }

            // The only remaining action is the removal of the permission from the function.
            var functionPermissionToRemove = functionModel.FunctionPermissions.Where(fp => fp.PermissionId == functionPermissionTransientModel.PermissionId).FirstOrDefault();
            functionModel.FunctionPermissions.Remove(functionPermissionToRemove);
        }

        private async Task<List<FunctionPermissionTransientModel>> DetectAndCapturePermissionsRemovedFromFunction(FunctionModel functionModel, Guid functionId, FunctionSubmit functionSubmit, Guid capturedBy, List<FunctionPermissionTransientModel> affectedFunctionPermissionTransientRecords)
        {
            var currentReleasedFunctionPermissions = functionModel.FunctionPermissions ?? new List<FunctionPermissionModel>();
            // Extract the IDs of the currently assigned permissions, as we want to iterate through this array, as opposed to the actual
            // function permissions collection, as we are looking to modify the function permissions collection.
            var currentReleasedFunctionPermissionIds = currentReleasedFunctionPermissions.Select(fp => fp.PermissionId).ToArray();

            foreach (var assignedPermissionId in currentReleasedFunctionPermissionIds)
            {
                var permissionIdFromSubmitList = functionSubmit.Permissions.Where(p => p == assignedPermissionId).FirstOrDefault();

                if (permissionIdFromSubmitList != Guid.Empty)
                {
                    // Continue if the currently assigned permission is within the function submit permission IDs.
                    continue;
                }

                // If this portion of the execution is reached, we have a permission that is currently assigned to the function but no longer
                // appears within the newly declared associated permissions list within the function submit. Capture a deletion of the currently assigned permission.
                var removedTransientFunctionPermissionRecord = await CaptureFunctionPermissionAssignmentChange(functionId, assignedPermissionId, capturedBy, TransientAction.Delete, functionSubmit.SubRealmId);
                CheckForAndProcessReleasedFunctionPermissionTransientRecord(functionModel, removedTransientFunctionPermissionRecord);
                affectedFunctionPermissionTransientRecords.Add(removedTransientFunctionPermissionRecord);
            }

            return affectedFunctionPermissionTransientRecords;
        }

        private async Task CheckForApplicationAndAssignToFunctionTransientIfExists(FunctionTransientModel functionTransient, Guid applicationId)
        {
            var application = await applicationRepository.GetByIdAsync(applicationId);

            if(application != null)
            {
                functionTransient.ApplicationId = application.Id;
            }
            else
            {
                throw new ItemNotFoundException($"Application with UUID: '{applicationId}' not found. Cannot assign this application to the function.");
            }
        }

        



        public async Task<Function> GetByIdAsync(Guid functionId)
        {
            return mapper.Map<Function>(await functionRepository.GetByIdAsync(functionId));
        }

        public async Task<List<Function>> GetListAsync()
        {
            return mapper.Map<List<Function>>(await functionRepository.GetListAsync());
        }

        public async Task<FunctionTransient> UpdateAsync(FunctionSubmit functionSubmit, Guid functionId, Guid updatedByGuid)
        {
            // Start transactions to allow complete rollback in case of an error
            InitSharedTransaction();

            try
            {
                FunctionModel existingFunction = await functionRepository.GetByIdAsync(functionId);

                if(existingFunction == null)
                {
                    throw new ItemNotFoundException($"Function with ID '{functionId}' not found.");
                }

                await CheckFunctionsAndTransientFunctionsForUniqueName(functionSubmit.Name, functionId);

                FunctionTransientModel newFunctionTransient = await CaptureTransientFunctionAsync(functionId, functionSubmit.Name, functionSubmit.Description, functionSubmit.ApplicationId, functionSubmit.SubRealmId, TransientAction.Modify, updatedByGuid);

                // Even though we are creating/capturing the function here, it is possible that the configured approval count is 0,
                // which means that we need to check for whether the transient state is released, and process the affected function accrodingly.
                // NOTE: It is possible for an empty function (not persisted) to be returned if the function is not released in the following step.
                FunctionModel function = await UpdateFunctionBasedOnTransientActionIfTransientFunctionStateIsReleased(newFunctionTransient);

                // If there was not update to the function (as a result of released state being processed), use the exiting state as the authoratative one.
                if(function.Id == Guid.Empty)
                {
                    function = existingFunction;
                }

                newFunctionTransient.LatestTransientFunctionPermissions = await CaptureFunctionPermissionAssignmentChanges(function, newFunctionTransient.FunctionId, functionSubmit, updatedByGuid, functionSubmit.SubRealmId);

                // It is possible that the assigned permisisons state has changed. Update the model, but only if it has an ID.
                if (function.Id != Guid.Empty)
                {
                    await functionRepository.UpdateAsync(function);
                }

                // All successful
                CommitTransaction();

                return mapper.Map<FunctionTransient>(newFunctionTransient);
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
        }

        public async Task DeleteAsync(Guid functionId)
        {
            var function = await functionRepository.GetByIdAsync(functionId);

            if (function == null)
            {
                throw new ItemNotFoundException($"Function with UUID '{functionId}' not found.");
            }

            await functionRepository.DeleteAsync(function);
        }

        public async Task<PaginatedResult<FunctionModel>> GetPaginatedListAsync(int page, int pageSize, bool includeRelations, string filterName, List<KeyValuePair<string, string>> orderBy)
        {
            return await functionRepository.GetPaginatedListAsync(page, pageSize, includeRelations, filterName, orderBy);
        }

        public void InitSharedTransaction()
        {
            permissionRepository.InitSharedTransaction();
            applicationRepository.InitSharedTransaction();
            functionRepository.InitSharedTransaction();
            subRealmRepository.InitSharedTransaction();
            functionPermissionTransientRepository.InitSharedTransaction();
            functionTransientRepository.InitSharedTransaction();    
        }

        public void CommitTransaction()
        {
            permissionRepository.CommitTransaction();
            applicationRepository.CommitTransaction();
            functionRepository.CommitTransaction();
            subRealmRepository.CommitTransaction();
            functionPermissionTransientRepository.CommitTransaction();
            functionTransientRepository.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            permissionRepository.RollbackTransaction();
            applicationRepository.RollbackTransaction();
            functionRepository.RollbackTransaction();
            subRealmRepository.RollbackTransaction();
            functionPermissionTransientRepository.RollbackTransaction();
            functionTransientRepository.RollbackTransaction();
        }
    }
}
