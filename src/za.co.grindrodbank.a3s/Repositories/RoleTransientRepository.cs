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
    public class RoleTransientRepository : IRoleTransientRepository
    {
        private readonly A3SContext a3SContext;

        public RoleTransientRepository(A3SContext a3SContext)
        {
            this.a3SContext = a3SContext;
        }

        public async Task<RoleTransientModel> CreateAsync(RoleTransientModel roleTransient)
        {
            a3SContext.RoleTransient.Add(roleTransient);
            await a3SContext.SaveChangesAsync();

            return roleTransient;
        }

        public async Task<List<RoleTransientModel>> GetTransientsForRoleAsync(Guid roleId)
        {
            return await a3SContext.RoleTransient.Where(rt => rt.RoleId == roleId)
                                                 .OrderBy(rt => rt.CreatedAt)
                                                 .ToListAsync();
        }

        public void InitSharedTransaction()
        {
            if (a3SContext.Database.CurrentTransaction == null)
                a3SContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (a3SContext.Database.CurrentTransaction != null)
                a3SContext.Database.CurrentTransaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (a3SContext.Database.CurrentTransaction != null)
                a3SContext.Database.CurrentTransaction.Rollback();
        }

        public async Task<List<RoleTransientModel>> GetLatestActiveTransientsForAllRolesAsync()
        {
            return await a3SContext.RoleTransient
                .FromSqlRaw("SELECT \"RoleTransient\".* " +
                            "FROM (SELECT DISTINCT ON (role_id) * FROM _a3s.role_transient ORDER BY role_id, created_at desc) AS \"RoleTransient\" " +
                            "WHERE r_state != 'Released' AND r_state != 'Declined';")
                            .ToListAsync();
        }

        public async Task<RoleTransientModel> GetLatestCapturedTransientForRoleAsync(Guid RoleId)
        {
            return await a3SContext.RoleTransient
                .FromSqlRaw("SELECT \"RoleTransient\".* " +
                            "FROM _a3s.role_transient AS \"RoleTransient\"  " +
                            "WHERE r_state = 'Captured' AND role_id = {0} " +
                            "ORDER BY created_at desc", RoleId)
                            .FirstOrDefaultAsync();
        }
    }
}
