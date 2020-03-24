/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
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
            var allBaseRoleTransients = await roleTransientRepository.GetTransientsForAllRolesAsync();

            // Start building the overall system transients map.
            foreach(var baseRoleActiveTransient in allBaseRoleTransients)
            {
                allSystemTransients.TransientRoles.Add(new SystemTransientsRoleModel
                {
                    RoleId = baseRoleActiveTransient.RoleId,
                    LatestActiveRoleTransient = baseRoleActiveTransient,
                    LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>(),
                    LatestActiveChildRoleTransients = new List<RoleRoleTransientModel>()
                });
            }

            var allRoleFunctionTransients = await roleFunctionTransientRepository.GetTransientsForAllRolesAsync();

            foreach(var roleFunctionTransient in allRoleFunctionTransients)
            {
                // Determine if the parent TransientRoles container object exists, create it if it doesn't.
                var existingTransientRoleContainerObject = allSystemTransients.TransientRoles.Where(tr => tr.RoleId == roleFunctionTransient.RoleId).FirstOrDefault();

                if(existingTransientRoleContainerObject == null)
                {
                    Console.WriteLine($"ADDING a completely new parent!");
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
                else // Parent may have been created by another transient.
                {
                    Console.WriteLine($"ADDING a role function to the  parent!");
                    if (existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients == null)
                    {
                        Console.WriteLine($"ROLEFUNCTION is NULL - ADDING!");
                        existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients = new List<RoleFunctionTransientModel>();
                    }
                    Console.WriteLine($"ADDING ROLEFUNCTION TO  CONTAINER!");
                    existingTransientRoleContainerObject.LatestActiveRoleFunctionTransients.Add(roleFunctionTransient);
                }
            }


}
    }
}
