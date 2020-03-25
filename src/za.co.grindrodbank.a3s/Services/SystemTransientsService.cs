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

namespace za.co.grindrodbank.a3s.Services
{
    public class SystemTransientsService : ISystemTransientsService
    {
        private readonly IRoleTransientRepository roleTransientRepository;
        private readonly IRoleFunctionTransientRepository roleFunctionTransientRepository;
        private readonly IRoleRoleTransientRepository roleRoleTransientRepository;

        public SystemTransientsService(IRoleTransientRepository roleTransientRepository, IRoleFunctionTransientRepository roleFunctionTransientRepository, IRoleRoleTransientRepository roleRoleTransientRepository)
        {
            this.roleTransientRepository = roleTransientRepository;
            this.roleFunctionTransientRepository = roleFunctionTransientRepository;
            this.roleRoleTransientRepository = roleRoleTransientRepository;
        }

        public async Task<SystemTransientsModel> GetAllSystemTransients(bool includeRoles = false)
        {
            SystemTransientsModel allSystemTransients = new SystemTransientsModel();
            allSystemTransients.TransientRoles = new List<SystemTransientsRoleModel>();

            await GetAllLatestTransientRoles(allSystemTransients);

            return allSystemTransients;
        }

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
                allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                {
                    RoleId = baseRoleActiveTransient.RoleId,
                    LatestActiveRoleTransient = baseRoleActiveTransient,
                    LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>(),
                    LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>()
                });
            }
        }

        private async Task GetAllRoleFunctionTransientsAndAddThemToTransientResponse(SystemTransientsModel allSystemTransients)
        {
            var allRoleFunctionTransients = await roleFunctionTransientRepository.GetLatestActiveTransientsForAllRolesAsync();

            foreach (var roleFunctionTransient in allRoleFunctionTransients)
            {
                // Determine if the parent TransientRoles container object exists, create it if it doesn't.
                var existingTransientRoleContainerObject = allSystemTransients.TransientRoles.Where(tr => tr.RoleId == roleFunctionTransient.RoleId).FirstOrDefault();

                if (existingTransientRoleContainerObject == null)
                {
                    allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                    {
                        RoleId = roleFunctionTransient.RoleId,
                        LatestActiveRoleTransient = new RoleTransientModel(),
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

            foreach (var roleRoleTransient in allRoleRoleTransients)
            {
                // Determine if the parent TransientRoles container object exists, create it if it doesn't.
                var existingTransientRoleContainerObject = allSystemTransients.TransientRoles.Where(tr => tr.RoleId == roleRoleTransient.ParentRoleId).FirstOrDefault();

                if (existingTransientRoleContainerObject == null)
                {
                    allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                    {
                        RoleId = roleRoleTransient.ParentRoleId,
                        LatestActiveRoleTransient = new RoleTransientModel(),
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
