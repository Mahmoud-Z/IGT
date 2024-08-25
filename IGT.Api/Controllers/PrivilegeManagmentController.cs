using IGT.Core.Dtos.PrivilegeManagment;
using IGT.Core.Dtos.RoleManagment;
using IGT.Service.Helpers.CustomAttributes;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IGT.Api.Controllers
{
    [CustomAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PrivilegeManagmentController : ControllerBase
    {
        private readonly IPrivilegeManagmentService _privilegeManagmentService;
        public PrivilegeManagmentController(IPrivilegeManagmentService privilegeManagmentService)
        {
            _privilegeManagmentService = privilegeManagmentService;
        }
        [HttpPost("getAllPrivilegesByRole")]
        public async Task<IActionResult> GetAllPrivilegesByRole()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userRole = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                var result = await _privilegeManagmentService.GetAllPrivilegesByRole(userRole);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
    }
}
