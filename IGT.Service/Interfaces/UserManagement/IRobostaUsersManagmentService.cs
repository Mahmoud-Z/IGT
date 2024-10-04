using IGT.Core.Dtos;
using IGT.Core.Dtos.UserManagment;
using IGT.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces.UserManagement
{
    public interface IRobostaUsersManagmentService
    {
        Task<BaseDTO<AuthenticationModel>> RobostaLogin(RobostaLoginInputDTO model);
        Task<BaseDTO<string>> RobostaRegister(RegisterModel model);
    }
}
