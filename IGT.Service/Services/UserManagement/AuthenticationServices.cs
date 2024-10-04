using IGT.Core.Dtos;
using IGT.Core.Dtos.UserManagment;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Helpers;
using IGT.Service.Helpers.EmailConfiguration;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers.Valiodators;
using IGT.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IGT.Service.Services.UserManagement
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtModel _jwt;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationServices(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JwtModel> jwt, 
            IConfiguration configuration, IEmailService emailService, IUnitOfWork unitOfWork )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _configuration = configuration;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseDTO<AuthenticationModel>> login(TokenRequestModel model)
        {
            try
            {
                var authModel = new AuthenticationModel();
                if (model.Username.Equals(_configuration.GetSection("SuperAdmin:Username").Value)
                    && model.Password.Equals(_configuration.GetSection("SuperAdmin:Password").Value))
                {
                    var jwtSecurityToken = await CreateJwtTokenForSuperADmin(model);
                    List<string> rolesList = [RolesEnum.SuperAdmin.ToString()];
                    authModel.IsAuthenticated = true;
                    authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    authModel.Username = model.Username;
                    authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                    authModel.Roles = rolesList.ToList();
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        throw new BussinessException(AuthenticationResource.EmailOrPasswordIsIncorrect);
                    }
                    await DestroySessions(user.Id);
                    if (user.isTempUser && user.expiresAt <= DateTime.Now)
                    {
                        throw new BussinessException(AuthenticationResource.ExpiredUser);
                    }
                    var jwtSecurityToken = await CreateJwtToken(user);
                    var rolesList = await _userManager.GetRolesAsync(user);
                    authModel.IsAuthenticated = true;
                    authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    authModel.Email = user.Email;
                    authModel.Username = user.UserName;
                    authModel.FirstLogin = user.FirstLogin;
                    authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                    authModel.Roles = rolesList.ToList();
                    await AddSession(authModel.Token,user);
                }
                return new BaseDTO<AuthenticationModel>
                {
                    IsSuccess = true,
                    Status = ResponseStatusEnum.Success.ToString(),
                    Data = authModel
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw new BussinessException(AuthenticationResource.GeneralError);
            }
            
        }
        public async Task<BaseDTO<string>> register(RegisterModel model)
        {
            try
            {
                // Make role be customer
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    throw new BussinessException(AuthenticationResource.EmailIsAlreadyRegistered);

                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    throw new BussinessException(AuthenticationResource.UsernameIsAlreadyRegistered);
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FirstLogin = false
                };
                if (await _roleManager.RoleExistsAsync(model.Role))
                {

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        var errors = string.Empty;

                        foreach (var error in result.Errors)
                            errors += $"{error.Description},";

                        throw new BussinessException(errors);
                    }
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                else
                {
                    throw new BussinessException(AuthenticationResource.ThisRoleDoesntExist);
                }
                return new BaseDTO<string>
                {
                    IsSuccess = true,
                    Data = AuthenticationResource.UserCreatedSuccessfully.Replace("$$", user.FirstName + " " + user.LastName),
                    Status = ResponseStatusEnum.Success.ToString(),
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }
        public async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim("username", user.UserName),
                new Claim("jti", Guid.NewGuid().ToString()),
                user.Email == null ? new Claim("phoneNumber", user.PhoneNumber) : new Claim("mail", user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        private async Task<JwtSecurityToken> CreateJwtTokenForSuperADmin(TokenRequestModel model)
        {
            var roleClaims = new List<Claim>();
            roleClaims.Add(new Claim("roles", "SuperAdmin"));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", _configuration.GetSection("SuperAdmin:UserId").Value)
            }
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<BaseDTO<string>> forgetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new BussinessException(AuthenticationResource.UserNotFound);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPassworedLink = MailResource.ForgetPasswordBody.Replace("$$", token);
                var message = new Message(new string[] {user.Email} , MailResource.ForgetPasswordSubject , forgotPassworedLink);
                _emailService.SendEmail(message);
                return new BaseDTO<string>
                {
                    IsSuccess = true,
                    Status = ResponseStatusEnum.Success.ToString(),
                    Data = GeneralResource.Success
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {

                throw new BussinessException(AuthenticationResource.GeneralError);
            }

        }
        public async Task<BaseDTO<string>> ResetPassword(ResetPasswordInputDTO input, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new BussinessException(AuthenticationResource.UserNotFound);
                }
                if (!input.NewPassword.Equals(input.ConfirmPassword))
                {
                    throw new BussinessException(AuthenticationResource.NewAndConfirmPasswordDoesntMatch);
                }
                var resetPassResult = await _userManager.ResetPasswordAsync(user,input.Token,input.NewPassword);
                if (!resetPassResult.Succeeded)
                {
                    throw new BussinessException(AuthenticationResource.GeneralError);
                }
                return new BaseDTO<string>
                {
                    IsSuccess = true,
                    Status = ResponseStatusEnum.Success.ToString(),
                    Data = GeneralResource.Success
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {

                throw new BussinessException(AuthenticationResource.GeneralError);
            }

        }
        public async Task<BaseDTO<string>> ChangePassword(ChangePasswordInputDTO input, string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    throw new BussinessException(AuthenticationResource.UserNotFound);
                }
                if (!input.NewPassword.Equals(input.ConfirmPassword))
                {
                    throw new BussinessException(AuthenticationResource.NewAndConfirmPasswordDoesntMatch);
                }
                var resetPassResult = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
                if (!resetPassResult.Succeeded)
                {
                    throw new BussinessException(AuthenticationResource.GeneralError);
                }
                if (user.FirstLogin)
                {
                    user.FirstLogin = false;
                    await _userManager.UpdateAsync(user);
                }
                return new BaseDTO<string>
                {
                    IsSuccess = true,
                    Status = ResponseStatusEnum.Success.ToString(),
                    Data = GeneralResource.Success
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {

                throw new BussinessException(AuthenticationResource.GeneralError);
            }

        }
        private async Task DestroySessions(string userId)
        {
            List<Session> sessions = (await _unitOfWork.GetRepository<Session>().FindAllAsync(s => s.User.Id == userId)).ToList();
            SystemStatusCode? systemStatusCode = _unitOfWork.SystemStatusCode.getExpiredGeneralStatus();
            sessions.ForEach(session => session.SystemStatusCode = systemStatusCode);
            _unitOfWork.GetRepository<Session>().UpdateRangeAsync(sessions);
            _unitOfWork.Complete();
        }
        public async Task AddSession(string token,User user)
        {
            SystemStatusCode? systemStatusCode = _unitOfWork.SystemStatusCode.getActiveGeneralStatus();
            Session session = new Session()
            {
                Token = token,
                User = user,
                SystemStatusCode = systemStatusCode
            };
            await _unitOfWork.GetRepository<Session>().AddAsync(session);
            await _unitOfWork.CompleteAsync();
        }
    }
}
