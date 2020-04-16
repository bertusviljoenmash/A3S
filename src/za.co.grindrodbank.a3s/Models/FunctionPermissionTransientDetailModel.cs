using System;
using static za.co.grindrodbank.a3s.Models.TransientStateMachineRecord;

namespace za.co.grindrodbank.a3s.Models
{
    public class FunctionPermissionTransientDetailModel
    {
            public Guid Id { get; set; }
            public Guid FunctionId { get; set; }
            public PermissionModel Permission { get; set; }
            public DatabaseRecordState R_State { get; set; }
            public Guid ChangedBy { get; set; }
            public int ApprovalCount { get; set; }
            public int RequiredApprovalCount { get; set; }
            public TransientAction Action { get; set; }

            public DateTime CreatedAt { get; set; }
    }
}
