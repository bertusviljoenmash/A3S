/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Repositories
{
    public interface IRoleRoleTransientRepository : ITransactableRepository
    {
        Task<List<RoleRoleTransientModel>> GetTransientChildRoleRelationsForRoleAsync(Guid roleId, Guid childRoleId);
        Task<List<RoleRoleTransientModel>> GetAllTransientChildRoleRelationsForRoleAsync(Guid roleId);
        Task<RoleRoleTransientModel> CreateNewTransientStateForRoleChildRoleAsync(RoleRoleTransientModel roleRoleTransient);
        Task<List<RoleRoleTransientModel>> GetLatestActiveTransientsForAllRolesAsync();
    }
}
