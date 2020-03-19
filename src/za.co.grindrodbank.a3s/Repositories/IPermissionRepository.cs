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
    public interface IPermissionRepository : ITransactableRepository, IPaginatedRepository<PermissionModel>
    {
        Task<PermissionModel> GetByNameAsync(string name, bool includeRelations = false);
        Task<PermissionModel> GetByIdAsync(Guid permissionId);
        Task<PermissionModel> GetByIdWithApplicationAsync(Guid permissionId);
        PermissionModel GetByName(string name, bool includeRelations = false);
        Task<PermissionModel> CreateAsync(PermissionModel permission);
        Task<PermissionModel> UpdateAsync(PermissionModel permission);
        Task Delete(PermissionModel permission);
        Task DeletePermissionsNotAssignedToApplicationFunctionsAsync();
        Task<List<PermissionModel>> GetListAsync();
        Task<PaginatedResult<PermissionModel>> GetPaginatedListAsync(int page, int pageSize, string filterName, List<KeyValuePair<string, string>> orderBy);
        Task<List<PermissionModel>> GetListAsync(Guid userId);
    }
}
