/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace za.co.grindrodbank.a3s.Repositories
{
    public class PaginatedRepository<T> : IPaginatedRepository<T> where T : class
    {
        public PaginatedRepository()
        {
        }

        public async Task<PaginatedResult<T>> GetPaginatedListFromQueryAsync(IQueryable<T> query, int page, int pageSize)
        {
            // Set default page and page size for all paginated lists here.
            // This should be pulled from configuration.
            page = page == 0 ? 1 : page;
            pageSize = pageSize == 0 ? 10 : pageSize;

            var result = new PaginatedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();

            return result;
        }
    }
}
