using IGT.Core.Dtos;
using IGT.Core.Dtos.RoleManagment;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Helpers;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Interfaces.RoleManagment;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Services.RoleManagment
{
    public class RoleManagmentService : IRoleManagmentService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public RoleManagmentService(RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseDTO<string>> AddRole(AddRoleInputDTO model)
        {
            try
            {
                if (model.Name != null && !await _roleManager.RoleExistsAsync(model.Name))
                {
                    var privileges = await _unitOfWork.GetRepository<Privilege>().FindAllAsync(p => model.PrivilegeCodes.Contains(p.Code));
                    if (privileges.Any())
                    {
                        var role = new Role { Name = model.Name,Privileges = privileges.ToList() };
                        var result = await _roleManager.CreateAsync(role);

                        if (!result.Succeeded)
                        {
                            var errors = string.Empty;

                            foreach (var error in result.Errors)
                                errors += $"{error.Description},";

                            throw new BussinessException(errors);
                        }
                        return new BaseDTO<string>
                        {
                            IsSuccess = true,
                            Data = $"Role with name {role.Name} has been created successfully",
                            Status = ResponseStatusEnum.Success.ToString(),
                        };
                    }
                    else
                    {
                        throw new BussinessException(RoleManagmentResource.PrivilegesNotFound);
                    }
                }
                else
                {
                    throw new BussinessException(RoleManagmentResource.RoleAlreadyExists);
                }
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
        public async Task<BaseDTO<string>> UpdateRole(AddRoleInputDTO model)
        {
            try
            {
                Role role = await _roleManager.FindByNameAsync(model.Name);
                if (model.Name != null && role != null)
                {
                    var privileges = (await _unitOfWork.GetRepository<Privilege>().FindAllAsync(p => model.PrivilegeCodes.Contains(p.Code))).ToList();
                    if (privileges.Any())
                    {
                        role.Name = model.Name;
                        role.Privileges = privileges;
                        var result = await _roleManager.UpdateAsync(role);

                        if (!result.Succeeded)
                        {
                            var errors = string.Empty;

                            foreach (var error in result.Errors)
                                errors += $"{error.Description},";

                            throw new BussinessException(errors);
                        }
                        return new BaseDTO<string>
                        {
                            IsSuccess = true,
                            Data = $"Role with name {role.Name} has been updated successfully",
                            Status = ResponseStatusEnum.Success.ToString(),
                        };
                    }
                    else
                    {
                        throw new BussinessException(RoleManagmentResource.PrivilegesNotFound);
                    }
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
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        public async Task<BaseDTO<string>> DeleteRole(string rolename)
        {
            try
            {
                Role role = await _roleManager.FindByNameAsync(rolename);
                if (rolename != null && role != null)
                {
                    role.SystemStatusCode = _unitOfWork.SystemStatusCode.GetAllSystemStatusCodes()
.FirstOrDefault(s => s.Status.Equals(SystemStatusCodeEnum.DELETED.ToString()) && s.Model.Equals(SystemStatusCodeEnum.GENERAL.ToString()));
                    var result = await _roleManager.UpdateAsync(role);

                    if (!result.Succeeded)
                    {
                        var errors = string.Empty;

                        foreach (var error in result.Errors)
                            errors += $"{error.Description},";

                        throw new BussinessException(errors);
                    }
                    return new BaseDTO<string>
                    {
                        IsSuccess = true,
                        Data = $"Role with name {role.Name} has been deleted successfully",
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
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        public async Task<BaseDTO<List<RolesDTO>>> GetRoles(string? id)
        {
            try
            {
                List<Role>? roles = (await _unitOfWork.GetRepository<Role>().FindAllAsync(r => (id == null || r.Id == id) &&
                (r.SystemStatusCode == null || r.SystemStatusCode.Status != SystemStatusCodeEnum.DELETED.ToString())
                && r.Name != RolesEnum.Admin.ToString(), new string []{ "Privileges", "SystemStatusCode" })).ToList();
                if (roles != null)
                {
                    List<RolesDTO> rolesDTOs = new List<RolesDTO>();
                    foreach (var role in roles)
                    {                                                                         
                        rolesDTOs.Add(new RolesDTO(role));
                    }
                    return new BaseDTO<List<RolesDTO>>
                    {
                        IsSuccess = true,
                        Data = rolesDTOs,
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
