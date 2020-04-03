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
    public interface IApplicationService
    {
        Task<Application> GetByIdAsync(Guid applicationId);
        Task<PaginatedResult<ApplicationModel>> GetListAsync(int page, int pageSize, string filterName, string filterFunctionName, List<KeyValuePair<string, string>> orderBy);
    }
}
