using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.UserManagment
{
    public class VerifyOTPOutputDTO
    {
        public bool? IsRegistered { get; set; }
        public string? Message { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
