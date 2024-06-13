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
    public interface IAuthenticationServices
    {
        Task<BaseDTO<AuthenticationModel>> login(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<BaseDTO<string>> forgetPassword(string email);
        Task<BaseDTO<string>> ResetPassword(ResetPasswordInputDTO input, string email);
    }
}
