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
    [Table("SubRealmPermission")]
    public class SubRealmPermissionModel : AuditableModel
    {
        public Guid SubRealmId { get; set; }
        public SubRealmModel SubRealm { get; set; }
        public Guid PermissionId { get; set; }
        public PermissionModel Permission { get; set; }
    }
}
