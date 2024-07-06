using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Data.Models
{
    public class SystemStatusCode
    {
        public long SystemStatusCodeId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Model { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
