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
    public interface IFunctionTransientRepository : ITransactableRepository
    {
        Task<List<FunctionTransientModel>> GetTransientsForFunctionAsync(Guid functionId);
        Task<List<FunctionTransientModel>> GetLatestActiveTransientsForAllFunctionsAsync();
        Task<FunctionTransientModel> GetLatestCapturedTransientForFunctionsAsync(Guid functionId);
        Task<FunctionTransientModel> CreateAsync(FunctionTransientModel functionTransient);
    }
}
