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
    public interface IFunctionRepository : ITransactableRepository, IPaginatedRepository<FunctionModel>
    {
        Task<FunctionModel> GetByNameAsync(string name);
        Task<FunctionModel> GetByIdAsync(Guid functionId);
        Task<FunctionModel> CreateAsync(FunctionModel function);
        Task<FunctionModel> UpdateAsync(FunctionModel function);
        Task DeleteAsync(FunctionModel function);
        Task<List<FunctionModel>> GetListAsync();
        Task<PaginatedResult<FunctionModel>> GetPaginatedListAsync(int page, int pageSize, bool includeRelations, string filterName, List<KeyValuePair<string, string>> orderBy);
    }
}
