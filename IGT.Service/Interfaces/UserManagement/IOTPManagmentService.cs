using IGT.Core.Dtos;
using IGT.Core.Dtos.UserManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces.UserManagement
{
    public interface IOTPManagmentService
    {
        Task<BaseDTO<string>> SendOTP(string phoneNumber);
        Task VerifyOTP(RobostaLoginInputDTO model);
    }
}
