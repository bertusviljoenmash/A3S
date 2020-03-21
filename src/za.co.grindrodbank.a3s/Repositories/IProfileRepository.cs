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
    public interface IProfileRepository : ITransactableRepository, IPaginatedRepository<ProfileModel>
    {
        Task<ProfileModel> GetByNameAsync(Guid userId, string name, bool includeRelations);
        Task<ProfileModel> GetByIdAsync(Guid profileId, bool includeRelations);
        Task<ProfileModel> CreateAsync(ProfileModel profile);
        Task<ProfileModel> UpdateAsync(ProfileModel profile);
        Task DeleteAsync(ProfileModel profile);
        Task<List<ProfileModel>> GetListAsync(bool includeRelations);
        Task<List<ProfileModel>> GetListForUserAsync(Guid userId, bool includeRelations);
        Task<PaginatedResult<ProfileModel>> GetPaginatedListForUserAsync(Guid userId, int page, int pageSize, bool includeRelations, string filterName, List<KeyValuePair<string, string>> orderBy);
    }
}
