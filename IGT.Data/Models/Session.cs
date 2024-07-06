using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Data.Models
{
    public class Session
    {
        public long SessionId { get; set; }
        public string? Token { get; set; }
        public long? SystemStatusCodeId { get; set; }
        public String? UserId { get; set; }
        public virtual SystemStatusCode? SystemStatusCode { get; set; }
        public virtual User? User { get; set; }
    }
}
