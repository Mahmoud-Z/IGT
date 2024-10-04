using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Data.Models
{
    public class OTP
    {
        public int OTPId { get; set; }
        public string OTPCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(-1.5);
        public bool IsUsed { get; set; } = false;
        //public string UserId { get; set; }
        //public virtual User? User { get; set; }
    }
}
