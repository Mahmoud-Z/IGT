using IGT.Core.Dtos.RoleManagment;
using IGT.Core.Dtos;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace IGT.Service.Services
{
    public class PrivilegeManagmentService : IPrivilegeManagmentService
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public PrivilegeManagmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseDTO<List<GetAllPrivilegesResponseDTO>>> GetAllPrivilegesByRole(string role)
        {
            try
            {
                List<Privilege>? privileges = (await _unitOfWork.GetRepository<Privilege>().FindAllAsync(p => (role == "SuperAdmin" || p.Roles.Any(r => r.Name == role)) && !p.IsSuperAdmin)).ToList();
                if (!privileges.IsNullOrEmpty())
                {
                    List<GetAllPrivilegesResponseDTO> privilegesDTOs = new List<GetAllPrivilegesResponseDTO>();
                    foreach (var privilege in privileges)
                    {
                        privilegesDTOs.Add(new GetAllPrivilegesResponseDTO(privilege));
                    }
                    return new BaseDTO<List<GetAllPrivilegesResponseDTO>>
                    {
                        IsSuccess = true,
                        Data = privilegesDTOs,
                        Status = ResponseStatusEnum.Success.ToString(),
                    };
                }
                else
                {
                    throw new BussinessException(RoleManagmentResource.ThisRoleDoesntExist);
                }
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
    }
}
