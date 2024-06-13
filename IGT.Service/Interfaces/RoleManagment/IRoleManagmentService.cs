using IGT.Core.Dtos.RoleManagment;
using IGT.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces.RoleManagment
{
    public interface IRoleManagmentService
    {
        Task<BaseDTO<string>> AddRole(AddRoleInputDTO model);
        Task<BaseDTO<string>> UpdateRole(AddRoleInputDTO model);
        Task<BaseDTO<string>> DeleteRole(string rolename);
        Task<BaseDTO<List<RolesDTO>>> GetRoles(string? id);
    }
}
