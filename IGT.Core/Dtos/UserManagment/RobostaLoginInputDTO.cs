using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.UserManagment
{
    public class RobostaLoginInputDTO
    {
        public string? phoneNumber { get; set; }
        public string? otpCode { get; set; }
    }
}
