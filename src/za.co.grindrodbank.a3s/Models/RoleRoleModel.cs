/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace za.co.grindrodbank.a3s.Models
{
    [Table("RoleRole")]
    public class RoleRoleModel : AuditableModel
{
        public Guid ParentRoleId { get; set; }
        public RoleModel ParentRole { get; set; }

        public Guid ChildRoleId { get; set; }
        public RoleModel ChildRole { get; set; }
    }
}
