﻿using IGT.Core.Dtos.RoleManagment;
using IGT.Service.Helpers;
using IGT.Service.Helpers.CustomAttributes;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers.Valiodators;
using IGT.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IGT.Api.Controllers
{
    [CustomAuthorize]
    public class RoleManagmentController : ControllerBase
    {
        private readonly IRoleManagmentService _roleManagmentService;
        public RoleManagmentController(IRoleManagmentService roleManagmentService)
        {
            _roleManagmentService = roleManagmentService;
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleInputDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _roleManagmentService.AddRole(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [HttpPost("updateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] AddRoleInputDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _roleManagmentService.UpdateRole(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [HttpPost("deleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] string roleName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _roleManagmentService.DeleteRole(roleName);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [HttpPost("getRoles")]
        public async Task<IActionResult> GetRoles([FromBody] string? roleId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _roleManagmentService.GetRoles(roleId);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
    }
}
