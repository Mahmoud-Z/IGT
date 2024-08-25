using IGT.Service.Helpers.CustomAttributes;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Helpers.Valiodators;
using IGT.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using IGT.Service.Interfaces;
using IGT.Core.Dtos.UserManagment;

namespace IGT.Api.Controllers
{
    [CustomAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : ControllerBase
    {
        private readonly IUserManagmentService _userManagmentService;
        public UserManagmentController(IUserManagmentService userManagmentService)
        {
            _userManagmentService = userManagmentService;
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> addUser([FromBody] CreateUserInputDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                ValidationHelper.ValidateRequiredProperties(model);
                ValidationHelper.IsValidEmail(model.Email);
                var result = await _userManagmentService.createUserByAdmin(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> getAllUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userRole = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                var result = await _userManagmentService.getAllUsers(userRole);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
    }
}
