/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Threading.Tasks;
using za.co.grindrodbank.a3s.Models;

namespace za.co.grindrodbank.a3s.Services
{
    public interface ISystemTransientsService
    {
        public Task<SystemTransientsModel> GetAllSystemTransients(bool includeRoles = false, bool includeFunctions = false, bool includeAuthModes = false, bool includeUsers = false);
    }
}
