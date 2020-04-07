/**
 * *************************************************
 * Copyright (c) 2020, Grindrod Bank Limited
 * License MIT: https://opensource.org/licenses/MIT
 * **************************************************
 */
﻿using System;
using static za.co.grindrodbank.a3s.Models.TransientStateMachineRecord;

namespace za.co.grindrodbank.a3s.Models
{
    /// <summary>
    /// Represents a transient role function assignment, but with the associated functions model populated.
    /// </summary>
    public class RoleFunctionTransientDetailModel
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public FunctionModel Function { get; set; }
        public DatabaseRecordState R_State { get; set; }
        public Guid ChangedBy { get; set; }
        public int ApprovalCount { get; set; }
        public int RequiredApprovalCount { get; set; }
        public TransientAction Action { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}