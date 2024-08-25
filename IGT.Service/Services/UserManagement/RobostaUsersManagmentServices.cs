using IGT.Core.Dtos;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IGT.Service.Interfaces;

namespace IGT.Service.Services.UserManagement
{
    public class RobostaUsersManagmentServices
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationServices _authenticationServices;
        public RobostaUsersManagmentServices(UserManager<User> userManager, IUnitOfWork unitOfWork, 
            IAuthenticationServices authenticationServices, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _authenticationServices = authenticationServices;
            _roleManager = roleManager;
        }
        public async Task<BaseDTO<AuthenticationModel>> Login(string phoneNumber)
        {
            try
            {
                var authModel = new AuthenticationModel();
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

                if (user == null)
                {
                    user = new User
                    {
                        UserName = phoneNumber,
                        PhoneNumber = phoneNumber
                    };
                    await _userManager.AddToRoleAsync(user, RolesEnum.Customer.ToString());
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        throw new BussinessException("Failed to create a new user.");
                    }
                }
                var jwtSecurityToken = await _authenticationServices.CreateJwtToken(user);
                var rolesList = await _userManager.GetRolesAsync(user);
                authModel.IsAuthenticated = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.Email = user.Email;
                authModel.Username = user.UserName;
                authModel.FirstLogin = user.FirstLogin;
                authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                authModel.Roles = rolesList.ToList();
                authModel.IsRegistered = false;
                await _authenticationServices.AddSession(authModel.Token, user);

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
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        public async Task<BaseDTO<string>> register(RegisterModel model)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
                if (user == null)
                    throw new BussinessException("Please verify phone number first");
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result = await _userManager.CreateAsync(user, model.Password);
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
    }
}
