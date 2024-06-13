using IGT.Core.Dtos;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers;
using IGT.Service.Interfaces.UserManagement;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using IGT.Core.Dtos.UserManagment;

namespace IGT.Service.Services.UserManagement
{
    public class UserManagmentService : IUserManagmentService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserManagmentService(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JwtModel> jwt, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<BaseDTO<string>> createUserByAdmin(RegisterModel model)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    throw new BussinessException(AuthenticationResource.EmailIsAlreadyRegistered);

                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    throw new BussinessException(AuthenticationResource.UsernameIsAlreadyRegistered);

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
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
                    Data = $"User with name {user.FirstName + " " + user.LastName} has been created successfully",
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
        public async Task<BaseDTO<List<GetAllUsersOutputDTO>>> getAllUsers(string? role)
        {
            try
            {
                var adminUserIds = (await _userManager.GetUsersInRoleAsync(role))
                .Select(u => u.Id)
                .ToList();
                var users = await _userManager.Users.Where(u => !adminUserIds.Contains(u.Id)).ToListAsync();
                BaseDTO<List<GetAllUsersOutputDTO>> output = new BaseDTO<List<GetAllUsersOutputDTO>>();
                output.Data = new List<GetAllUsersOutputDTO>();
                foreach (var user in users)
                {
                    GetAllUsersOutputDTO getAllUsersOutputDTO = new GetAllUsersOutputDTO(user);
                    getAllUsersOutputDTO.Role = (await _userManager.GetRolesAsync(user))[0];
                    output.Data.Add(getAllUsersOutputDTO);
                }
                return output;
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
