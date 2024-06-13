using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Data.Models
{
    public class Privilege
    {
        public long PrivilegeId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string BackendURL { get; set; } = null!;
        public bool IsGeneral { get; set; } = false;
        public bool IsSuperAdmin { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
