using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Data.Models
{
    public class Role : IdentityRole
    {
        public long? SystemStatusCodeId { get; set; }
        public string? CreatedUserId { get; set; }
        public virtual SystemStatusCode? SystemStatusCode { get; set; }
        public virtual ICollection<Privilege> Privileges { get; set; } = new List<Privilege>();
    }
}
