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
    public interface IFunctionPermissionTransientRepository : ITransactableRepository
    {
        Task<List<FunctionPermissionTransientModel>> GetTransientPermissionRelationsForFunctionAsync(Guid functionId, Guid permissionId);
        Task<List<FunctionPermissionTransientModel>> GetAllTransientPermissionRelationsForFunctionAsync(Guid functionId);
        Task<FunctionPermissionTransientModel> CreateNewTransientStateForFunctionPermissionAsync(FunctionPermissionTransientModel functionPermissionTransient);
        Task<List<FunctionPermissionTransientModel>> GetLatestActiveTransientsForAllFunctionsAsync();
    }
}
