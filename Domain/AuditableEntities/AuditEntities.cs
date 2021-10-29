using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.AuditableEntities
{
    public class AuditEntities
    {
        public DateTime created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletable { get; set; }
    }
}
