using System;
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
