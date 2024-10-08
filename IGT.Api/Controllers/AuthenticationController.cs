﻿using IGT.Core.Dtos.UserManagment;
using IGT.Service.Helpers;
using IGT.Service.Helpers.CustomAttributes;
using IGT.Service.Helpers.EmailConfiguration;
using IGT.Service.Helpers.Exceptions;
using IGT.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IGT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IEmailService _emailService;

        public AuthenticationController(IAuthenticationServices authenticationServices, IEmailService emailService)
        {
            _authenticationServices = authenticationServices;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] TokenRequestModel model)
        {
            //We need to cahnge the expiration time for the token
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _authenticationServices.login(model);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    try
        //    {
        //        var result = await _authenticationServices.register(model);

        //        return Ok(result);
        //    }
        //    catch (BussinessException ex)
        //    {
        //        return Ok(ex.ErrorObject);
        //    }
        //}

        [CustomAuthorize]
        [HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userEmail = User.FindFirst("mail")?.Value;
                var result = await _authenticationServices.forgetPassword(userEmail);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [CustomAuthorize]
        [HttpPost("forgetPassword2")]
        public async Task<IActionResult> ForgetPassword2([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userEmail = User.FindFirst("mail")?.Value;
                var result = await _authenticationServices.forgetPassword(userEmail);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [CustomAuthorize]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userEmail = User.FindFirst("mail")?.Value;
                var result = await _authenticationServices.ResetPassword(input,userEmail);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputDTO input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userEmail = User.FindFirst("mail")?.Value;
                var result = await _authenticationServices.ChangePassword(input, userEmail);

                return Ok(result);
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }

        [CustomAuthorize]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationServices.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [CustomAuthorize]
        [HttpGet("testEmail")]
        public async Task<IActionResult> testEmail()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var message = new Message(new string[] { "manomido7575@gmail.com" }, "Test", "<h1>Hello from the other side</h1>");
                _emailService.SendEmail(message);
                return Ok("result");
            }
            catch (BussinessException ex)
            {
                return Ok(ex.ErrorObject);
            }
        }
    }
}
