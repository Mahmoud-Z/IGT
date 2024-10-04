using IGT.Core.Dtos.UserManagment;
using IGT.Service.Helpers;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Interfaces.UserManagement;
using Microsoft.AspNetCore.Mvc;

namespace IGT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobostaUsersManagmentController : ControllerBase
    {
        private readonly IRobostaUsersManagmentService _robostaUsersManagmentService;
        private readonly IOTPManagmentService _iOTPManagmentService;

        public RobostaUsersManagmentController(IRobostaUsersManagmentService robostaUsersManagmentService, IOTPManagmentService iOTPManagmentService)
        {
            _robostaUsersManagmentService = robostaUsersManagmentService;
            _iOTPManagmentService = iOTPManagmentService;
        }

        [HttpPost("robostaLogin")]
        public async Task<IActionResult> RobostaLogin(RobostaLoginInputDTO model)
        {
            //We need to cahnge the expiration time for the token
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _robostaUsersManagmentService.RobostaLogin(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }

        [HttpPost("robostaRegister")]
        public async Task<IActionResult> RobostaRegister(RegisterModel model)
        {
            //We need to cahnge the expiration time for the token
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _robostaUsersManagmentService.RobostaRegister(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }

        [HttpPost("sendOTP")]
        public async Task<IActionResult> SendOTP(string phoneNumber)
        {
            //We need to cahnge the expiration time for the token
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _iOTPManagmentService.SendOTP(phoneNumber);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
    }
}
