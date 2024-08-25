using IGT.Core.Dtos.RoleManagment;
using IGT.Core.Dtos;
using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces
{
    public interface IPrivilegeManagmentService
    {
        Task<BaseDTO<List<GetAllPrivilegesResponseDTO>>> GetAllPrivilegesByRole(string roleId);
    }
}
