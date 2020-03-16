/**
 * *************************************************
 * Copyright (c) 2019, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using System.ComponentModel.DataAnnotations;

namespace za.co.grindrodbank.a3s.Models
{
    public class UserCustomAttributeModel
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public UserModel User { get; set; }
    }
}
