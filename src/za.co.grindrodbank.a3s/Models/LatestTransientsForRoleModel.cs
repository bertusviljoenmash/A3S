/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
using System.Collections.Generic;

namespace za.co.grindrodbank.a3s.Models
{
    public class LatestActiveTransientsForRoleModel
    {
        public List<RoleTransientModel> LatestActiveRoleTransients { get; set; }
        public List<RoleFunctionTransientDetailModel> LatestActiveRoleFunctionTransients { get; set; }
        public List<RoleRoleTransientDetailModel> LatestActiveChildRoleTransients { get; set; }
    }
}
