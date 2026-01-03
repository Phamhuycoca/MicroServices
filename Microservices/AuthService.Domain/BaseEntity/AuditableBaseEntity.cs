using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.BaseEntity
{
    public abstract class AuditableBaseEntity
    {
        public virtual Guid id { get; set; }
        public string? nguoi_tao { get; set; } = string.Empty;
        public DateTime ngay_tao { get; set; }
        public string? nguoi_chinh_sua { get; set; } = string.Empty;
        public DateTime? ngay_chinh_sua { get; set; }
    }
}
