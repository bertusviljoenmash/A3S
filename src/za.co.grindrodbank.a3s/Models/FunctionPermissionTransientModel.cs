/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.ComponentModel.DataAnnotations;

namespace za.co.grindrodbank.a3s.Models
{
    public class FunctionPermissionTransientModel 
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid FunctionId { get; set; }
        [Required]
        public Guid PermissionId { get; set; }
    }
}
