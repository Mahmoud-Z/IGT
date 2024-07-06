using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IGT.Service.Interfaces;
using IGT.Data.Models;
using IGT.Data.DatabaseContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IGT.Core.Enums;

namespace IGT.Service.Helpers.CustomAttributes
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private AkramDbContext _context;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _context = context.HttpContext.RequestServices.GetRequiredService<AkramDbContext>();
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            // Custom authorization logic
            if (!IsUserAuthorized(context).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        protected virtual async Task<bool> IsUserAuthorized(AuthorizationFilterContext context)
        {
            string endpoint = context.HttpContext.Request.RouteValues["controller"] + "/" + context.HttpContext.Request.RouteValues["action"];
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authorizationHeader?.Substring("Bearer ".Length).Trim();
            var roleClaims = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var username = context.HttpContext.User.Claims.Where(c => c.Type == "username").Select(c => c.Value).FirstOrDefault();
            if (roleClaims[0].Equals(RolesEnum.SuperAdmin.ToString()))
            {
                return true;
            }
            Session session = _context.Sessions.Where(s => s.User.UserName.Equals(username)).OrderBy(s => s.SessionId).LastOrDefault();
            if (session != null && !session.Token.Equals(token))
            {
                return false;
            }
            List<string> privileges = _context.Privileges
                        .Where(p => (p.Roles.Any(r => r.Name == roleClaims[0]) || p.IsGeneral) && !p.IsSuperAdmin)
                        .Select(p => p.BackendURL)
                        .ToList();
            if (privileges.Contains(endpoint))
            {
                return true;
            }
            // Implement your custom authorization logic here
            return false; // For demo purposes, always return true
        }
    }
}
