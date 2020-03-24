/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;

namespace za.co.grindrodbank.a3s.Models
{
    public class SystemTransientsRoleModel
    {
        public RoleTransientModel LatestActiveRoleTransient { get; set; }
        public RoleFunctionTransientModel LatestActiveRoleFunctionTransient { get; set; }
        public RoleRoleTransientModel LatestActiveChildRoleTransient { get; set; }
        public Guid RoleId { get; set; }
    }
}
