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
    public class FunctionTransientModel : TransientStateMachineRecord
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid FunctionId { get; set; }

        [Required]
        public Guid SubRealmId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [NotMapped]
        public List<FunctionPermissionTransientModel> LatestTransientFunctionPermissions { get; set; }
    }
}
