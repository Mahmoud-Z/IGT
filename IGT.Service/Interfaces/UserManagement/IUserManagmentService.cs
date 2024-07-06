using IGT.Core.Dtos;
using IGT.Core.Dtos.UserManagment;
using IGT.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces
{
    public interface IUserManagmentService
    {
        Task<BaseDTO<string>> createUserByAdmin(CreateUserInputDTO model);
        Task<BaseDTO<List<GetAllUsersOutputDTO>>> getAllUsers(string? role);
    }
}
