/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Repositories
{
    public class FunctionTransientRepository : IFunctionTransientRepository
    {
        private readonly A3SContext a3SContext;

        public FunctionTransientRepository(A3SContext a3SContext)
        {
            this.a3SContext = a3SContext;
        }

        public async Task<FunctionTransientModel> CreateAsync(FunctionTransientModel functionTransient)
        {
            a3SContext.FunctionTransient.Add(functionTransient);
            await a3SContext.SaveChangesAsync();

            return functionTransient;
        }

        public async Task<List<FunctionTransientModel>> GetLatestActiveTransientsForAllFunctionsAsync()
        {
            return await a3SContext.FunctionTransient
                .FromSqlRaw("SELECT \"FunctionTransient\".* " +
                            "FROM (SELECT DISTINCT ON (function_id) * FROM _a3s.function_transient ORDER BY function_id, created_at desc) AS \"FunctionTransient\" " +
                            "WHERE r_state != 'Released' AND r_state != 'Declined';")
                            .ToListAsync();
        }

        public async Task<FunctionTransientModel> GetLatestCapturedTransientForFunctionsAsync(Guid functionId)
        {
            return await a3SContext.FunctionTransient
                .FromSqlRaw("SELECT \"FucntionTransient\".* " +
                            "FROM _a3s.function_transient AS \"FunctionTransient\"  " +
                            "WHERE r_state = 'Captured' AND function_id = {0} " +
                            "ORDER BY created_at desc", functionId)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<FunctionTransientModel>> GetTransientsForFunctionAsync(Guid functionId)
        {
            return await a3SContext.FunctionTransient.Where(ft => ft.FunctionId == functionId)
                                                 .OrderBy(rt => rt.CreatedAt)
                                                 .ToListAsync();
        }

        public void CommitTransaction()
        {
            if (a3SContext.Database.CurrentTransaction != null)
                a3SContext.Database.CurrentTransaction.Commit();
        }

        public void InitSharedTransaction()
        {
            if (a3SContext.Database.CurrentTransaction == null)
                a3SContext.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (a3SContext.Database.CurrentTransaction != null)
                a3SContext.Database.CurrentTransaction.Rollback();
        }
    }
}
