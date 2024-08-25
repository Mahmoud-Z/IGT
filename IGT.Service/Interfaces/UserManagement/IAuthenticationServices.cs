using IGT.Core.Dtos;
using IGT.Core.Dtos.UserManagment;
using IGT.Data.Models;
using IGT.Service.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<BaseDTO<AuthenticationModel>> login(TokenRequestModel model);
        Task<BaseDTO<string>> register(RegisterModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<BaseDTO<string>> forgetPassword(string email);
        Task<BaseDTO<string>> ResetPassword(ResetPasswordInputDTO input, string email);
        Task<BaseDTO<string>> ChangePassword(ChangePasswordInputDTO input, string email);
        Task AddSession(string token, User user);
        Task<JwtSecurityToken> CreateJwtToken(User user);
    }
}
