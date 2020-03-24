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
        public List<RoleFunctionTransientModel> LatestActiveRoleFunctionTransients { get; set; }
        public List<RoleRoleTransientModel> LatestActiveChildRoleTransients { get; set; }
        public Guid RoleId { get; set; }
    }
}
