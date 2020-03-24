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

    public interface IRoleTransientRepository : ITransactableRepository
    {
        Task<List<RoleTransientModel>> GetTransientsForRoleAsync(Guid roleId);
        Task<List<RoleTransientModel>> GetLatestActiveTransientsForAllRolesAsync();
        Task<RoleTransientModel> CreateAsync(RoleTransientModel roleTransient);
    }
}
