using IGT.Core.Dtos;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers;
using IGT.Service.Interfaces;
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
using IGT.Service.Helpers.EmailConfiguration;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace IGT.Service.Services.UserManagement
{
    public class UserManagmentService : IUserManagmentService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEmailService _emailService;
        private const int PasswordLength = 12;
        public UserManagmentService(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<JwtModel> jwt, IConfiguration configuration
            , IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public async Task<BaseDTO<string>> createUserByAdmin(CreateUserInputDTO model)
        {
            try
            {
                if ((await _userManager.FindByEmailAsync(model.Email)) is not null)
                    throw new BussinessException(AuthenticationResource.EmailIsAlreadyRegistered);
                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    throw new BussinessException(AuthenticationResource.UsernameIsAlreadyRegistered);
                if (model.isTempUser && model.expiresAt < DateTime.Now)
                    throw new BussinessException(AuthenticationResource.ExpireDateWrong);
                string password = GeneratePassword();
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    isTempUser = model.isTempUser,
                    expiresAt = model.isTempUser ? model.expiresAt : null,
                };
                if (await _roleManager.RoleExistsAsync(model.Role))
                {

                    var result = await _userManager.CreateAsync(user, password);

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
                sendPassword(password, model.Email);
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
            catch (Exception ex)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        private void sendPassword(string password, string email)
        {
            var newPassword = "Kindly find your password here : " + password;
            var message = new Message(new string[] { email }, "Account Password", newPassword);
            _emailService.SendEmail(message);
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
        private string GeneratePassword()
        {
            // Define the character sets
            const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string DigitChars = "0123456789";
            const string NonAlphanumericChars = "!@#$%^&*()_-+=<>?";

            // Ensure password has at least one of each required character type
            StringBuilder password = new StringBuilder();
            password.Append(GetRandomChar(UppercaseChars));
            password.Append(GetRandomChar(DigitChars));
            password.Append(GetRandomChar(NonAlphanumericChars));

            // Fill the remaining password length with random characters from all sets
            string allChars = UppercaseChars + LowercaseChars + DigitChars + NonAlphanumericChars;
            for (int i = password.Length; i < PasswordLength; i++)
            {
                password.Append(GetRandomChar(allChars));
            }

            // Shuffle the password to ensure randomness
            return ShufflePassword(password.ToString());
        }
        private char GetRandomChar(string characterSet)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[1];
                rng.GetBytes(randomNumber);
                return characterSet[randomNumber[0] % characterSet.Length];
            }
        }
        private string ShufflePassword(string password)
        {
            Random random = new Random();
            return new string(password.OrderBy(c => random.Next()).ToArray());
        }
    }
}
