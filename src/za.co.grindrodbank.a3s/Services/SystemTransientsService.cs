/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;
using za.co.grindrodbank.a3s.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace za.co.grindrodbank.a3s.Services
{
    public class SystemTransientsService : ISystemTransientsService
    {
        private readonly IRoleTransientRepository roleTransientRepository;
        private readonly IRoleFunctionTransientRepository roleFunctionTransientRepository;
        private readonly IRoleRoleTransientRepository roleRoleTransientRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IUserRepository userRepository;

        public SystemTransientsService(IRoleTransientRepository roleTransientRepository, IRoleFunctionTransientRepository roleFunctionTransientRepository, IRoleRoleTransientRepository roleRoleTransientRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            this.roleTransientRepository = roleTransientRepository;
            this.roleFunctionTransientRepository = roleFunctionTransientRepository;
            this.roleRoleTransientRepository = roleRoleTransientRepository;
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
        }

        public async Task<SystemTransientsModel> GetAllSystemTransients(bool includeRoles = false, bool includeFunctions = false, bool includeAuthModes = false, bool includeUsers = false)
        {
            SystemTransientsModel allSystemTransients = new SystemTransientsModel
            {
                TransientRoles = new List<SystemTransientsRoleModel>()
            };

            if (includeUsers)
            {
                await GetAllLatestTransientRoles(allSystemTransients);
            }

            return allSystemTransients;
        }

        [Authorize(Policy = "permission:a3s.roles.read")]
        private async Task GetAllLatestTransientRoles(SystemTransientsModel allSystemTransients)
        {
            await GetAllBaseRoleTransientsAndAddThemToTransientsResponse(allSystemTransients);
            await GetAllRoleFunctionTransientsAndAddThemToTransientResponse(allSystemTransients);
            await GetAllRoleChildRoleTransientsAndAddThemToTransientResponse(allSystemTransients);
        }

        private async Task GetAllBaseRoleTransientsAndAddThemToTransientsResponse(SystemTransientsModel allSystemTransients)
        {
            var allBaseRoleTransients = await roleTransientRepository.GetLatestActiveTransientsForAllRolesAsync();

            // Start building the overall system transients map.
            foreach (var baseRoleActiveTransient in allBaseRoleTransients)
            {
                string roleName = "";
                DateTime capturedDate;

                // Recall, the role will not exists if it's creation has been captured.
                if(baseRoleActiveTransient.Action != TransientStateMachineRecord.TransientAction.Create)
                {
                    // Get the name of the role for the transient.
                    var role = await roleRepository.GetByIdAsync(baseRoleActiveTransient.RoleId);

                    if (role == null)
                    {
                        throw new ItemNotFoundException($"Role with ID: {baseRoleActiveTransient.RoleId} not found.");
                    }

                    roleName = role.Name;
                }

                Guid capturerGuid;
                
                // It is possible that the latest retrieved transient is the captured one. Get the capturer GUID from there, rather
                // than needing to search for the captured transient record first.
                if(baseRoleActiveTransient.R_State == TransientStateMachineRecord.DatabaseRecordState.Captured)
                {
                    capturerGuid = baseRoleActiveTransient.ChangedBy;
                    if (string.IsNullOrEmpty(roleName))
                    {
                        roleName = baseRoleActiveTransient.Name;
                    }

                    capturedDate = baseRoleActiveTransient.CreatedAt;
                }
                else // Find the most recent captured record for this transient.
                {
                    var mostRecentCapturedRoleTransient = await roleTransientRepository.GetLatestCapturedTransientForRoleAsync(baseRoleActiveTransient.RoleId);
                    if(mostRecentCapturedRoleTransient == null)
                    {
                        throw new ItemNotFoundException($"Latest Active Role Transient for role with ID: {baseRoleActiveTransient.RoleId} not found.");
                    }

                    capturerGuid = mostRecentCapturedRoleTransient.ChangedBy;

                    if (string.IsNullOrEmpty(roleName))
                    {
                        roleName = baseRoleActiveTransient.Name;
                    }

                    capturedDate = mostRecentCapturedRoleTransient.CreatedAt;
                }

                // Get the name of the user associated with the captured record.
                var capturer = await userRepository.GetByIdAsync(capturerGuid, false);
                var capturerName = $"{capturer.FirstName} {capturer.Surname}";

                allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                {
                    RoleId = baseRoleActiveTransient.RoleId,
                    RoleName = roleName,
                    RequesterName = capturerName,
                    RequesterGuid = capturerGuid,
                    RequestedDate = capturedDate,
                    LatestActiveRoleTransient = baseRoleActiveTransient,
                    LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>(),
                    LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>()
                });
            }
        }

        private async Task GetAllRoleFunctionTransientsAndAddThemToTransientResponse(SystemTransientsModel allSystemTransients)
        {
            var allRoleFunctionTransients = await roleFunctionTransientRepository.GetLatestActiveTransientsForAllRolesAsync();
            DateTime capturedDate;

            foreach (var roleFunctionTransient in allRoleFunctionTransients)
            {
                // Determine if the parent TransientRoles container object exists, create it if it doesn't.
                var existingTransientRoleContainerObject = allSystemTransients.TransientRoles.Where(tr => tr.RoleId == roleFunctionTransient.RoleId).FirstOrDefault();

                if (existingTransientRoleContainerObject == null)
                {
                    // Get the name of the role for the transient.
                    var role = await roleRepository.GetByIdAsync(roleFunctionTransient.RoleId);
                    Guid capturerGuid;

                    // It is possible that the latest retrieved transient is the captured one. Get the capturer GUID from there, rather
                    // than needing to search for the captured transient record first.
                    if (roleFunctionTransient.R_State == TransientStateMachineRecord.DatabaseRecordState.Captured)
                    {
                        capturerGuid = roleFunctionTransient.ChangedBy;
                        capturedDate = roleFunctionTransient.CreatedAt;
                    }
                    else // Find the most recent captured record for this transient.
                    {
                        var mostRecentCapturedRoleTransient = await roleTransientRepository.GetLatestCapturedTransientForRoleAsync(roleFunctionTransient.RoleId);
                        capturerGuid = mostRecentCapturedRoleTransient.ChangedBy;
                        capturedDate = mostRecentCapturedRoleTransient.CreatedAt;
                    }

                    // Get the name of the user associated with the captured record.
                    var capturer = await userRepository.GetByIdAsync(capturerGuid, false);
                    var capturerName = $"{capturer.FirstName} {capturer.Surname}";

                    allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                    {
                        RoleId = roleFunctionTransient.RoleId,
                        RoleName = role.Name,
                        RequesterName = capturerName,
                        RequesterGuid = capturerGuid,
                        RequestedDate = capturedDate,
                        LatestActiveRoleTransient = null,
                        LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel> {
                            roleFunctionTransient
                        },
                        LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>()
                    });
                }
                else // Parent may have been created by another transient component of roles.
                {
                    if (existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients == null)
                    {
                        existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>();
                    }

                    existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients.Add(roleFunctionTransient);
                }
            }
        }

        private async Task GetAllRoleChildRoleTransientsAndAddThemToTransientResponse(SystemTransientsModel allSystemTransients)
        {
            var allRoleRoleTransients = await roleRoleTransientRepository.GetLatestActiveTransientsForAllRolesAsync();
            DateTime capturedDate;

            foreach (var roleRoleTransient in allRoleRoleTransients)
            {
                // Determine if the parent TransientRoles container object exists, create it if it doesn't.
                var existingTransientRoleContainerObject = allSystemTransients.TransientRoles.Where(tr => tr.RoleId == roleRoleTransient.ParentRoleId).FirstOrDefault();

                if (existingTransientRoleContainerObject == null)
                {
                    // Get the name of the role for the transient.
                    var role = await roleRepository.GetByIdAsync(roleRoleTransient.ParentRoleId);
                    Guid capturerGuid;

                    // It is possible that the latest retrieved transient is the captured one. Get the capturer GUID from there, rather
                    // than needing to search for the captured transient record first.
                    if (roleRoleTransient.R_State == TransientStateMachineRecord.DatabaseRecordState.Captured)
                    {
                        capturerGuid = roleRoleTransient.ChangedBy;
                        capturedDate = roleRoleTransient.CreatedAt;
                    }
                    else // Find the most recent captured record for this transient.
                    {
                        var mostRecentCapturedRoleTransient = await roleTransientRepository.GetLatestCapturedTransientForRoleAsync(roleRoleTransient.ParentRoleId);
                        capturerGuid = mostRecentCapturedRoleTransient.ChangedBy;
                        capturedDate = mostRecentCapturedRoleTransient.CreatedAt;
                    }

                    // Get the name of the user associated with the captured record.
                    var capturer = await userRepository.GetByIdAsync(capturerGuid, false);
                    var capturerName = $"{capturer.FirstName} {capturer.Surname}";


                    allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                    {
                        RoleId = roleRoleTransient.ParentRoleId,
                        RoleName = role.Name,
                        RequesterName = capturerName,
                        RequesterGuid = capturerGuid,
                        RequestedDate = capturedDate,
                        LatestActiveRoleTransient = null,
                        LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>(),
                        LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>
                        {
                            roleRoleTransient
                        }
                    });
                }
                else // Parent may have been created by another transient component of roles.
                {
                    if (existingTransientRoleContainerObject.LatestActiveChildRoleTransients == null)
                    {
                        existingTransientRoleContainerObject.LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>();
                    }

                    existingTransientRoleContainerObject.LatestActiveChildRoleTransients.Add(roleRoleTransient);
                }
            }
        }
    }
}
