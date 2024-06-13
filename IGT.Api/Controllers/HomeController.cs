using IGT.Service.Helpers;
using IGT.Service.Helpers.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace IGT.Api.Controllers
{
    [CustomAuthorizeAttribute]
    public class HomeController : Controller
    {
        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok("sdgsdgsd");
        }
    }
}
