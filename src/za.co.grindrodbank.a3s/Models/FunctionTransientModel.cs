/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace za.co.grindrodbank.a3s.Models
{
    [Table("FunctionTransient")]
    public class FunctionTransient : TransientStateMachineRecord
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid SubRealmId { get; set; }

        [NotMapped]
        public List<FunctionPermissionTransientModel> LatestTransientRolePermissions { get; set; }
    }
}
