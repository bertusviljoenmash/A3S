/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.A3SApiResources;
using za.co.grindrodbank.a3s.Models;
using za.co.grindrodbank.a3s.Repositories;

namespace za.co.grindrodbank.a3s.Services
{
    public interface IRoleService : ITransactableService
    {
        Task<Role> GetByIdAsync(Guid roleId);
        Task<RoleTransient> UpdateAsync(RoleSubmit roleSubmit, Guid roleId, Guid updatedById);
        Task<RoleTransient> CreateAsync(RoleSubmit roleSubmit, Guid createdById);
        Task<List<Role>> GetListAsync();
        Task<PaginatedResult<RoleModel>> GetPaginatedListAsync(int page, int pageSize, bool includeRelations, string filterName, List<KeyValuePair<string, string>> orderBy);
        Task<RoleTransient> ApproveRoleAsync(Guid roleId, Guid approvedBy);
        Task<RoleTransient> DeclineRoleAsync(Guid roleId, Guid approvedBy);
        Task<RoleTransient> DeleteAsync(Guid roleId, Guid deletedById);
        Task<RoleTransients> GetLatestRoleTransientsAsync(Guid roleId);
    }
}
