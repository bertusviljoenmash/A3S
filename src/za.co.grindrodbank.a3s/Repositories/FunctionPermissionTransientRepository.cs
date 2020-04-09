using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Repositories
{
    public class FunctionPermissionTransientRepository : IFunctionPermissionTransientRepository
    {
        private readonly A3SContext a3SContext;

        public FunctionPermissionTransientRepository(A3SContext a3SContext)
        {
            this.a3SContext = a3SContext;
        }

        public async Task<FunctionPermissionTransientModel> CreateNewTransientStateForFunctionPermissionAsync(FunctionPermissionTransientModel functionPermissionTransient)
        {
            a3SContext.FunctionPermissionTransient.Add(functionPermissionTransient);

            await a3SContext.SaveChangesAsync();

            return functionPermissionTransient;
        }

        public async Task<List<FunctionPermissionTransientModel>> GetAllTransientPermissionRelationsForFunctionAsync(Guid functionId)
        {
            return await a3SContext.FunctionPermissionTransient
                                   .Where(fpt => fpt.FunctionId == functionId)
                                   .OrderBy(fpt => fpt.CreatedAt).ToListAsync();
        }

        public async Task<List<FunctionPermissionTransientModel>> GetLatestActiveTransientsForAllFunctionsAsync()
        {
            return await a3SContext.FunctionPermissionTransient
                .FromSqlRaw("SELECT \"FunctionPermissionTransient\".* " +
                            "FROM (SELECT DISTINCT ON (function_id) * FROM _a3s.function_permission_transient ORDER BY function_id, created_at desc) AS \"FunctionPermissionTransient\" " +
                            "WHERE r_state != 'Released' AND r_state != 'Declined';")
                            .ToListAsync();
        }

        public async Task<List<FunctionPermissionTransientModel>> GetTransientPermissionRelationsForFunctionAsync(Guid functionId, Guid permissionId)
        {
            return await a3SContext.FunctionPermissionTransient
                                   .Where(fpt => fpt.PermissionId == permissionId)
                                   .Where(fpt => fpt.FunctionId == functionId)
                                   .OrderBy(fpt => fpt.CreatedAt).ToListAsync();
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
