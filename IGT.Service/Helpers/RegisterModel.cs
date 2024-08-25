using IGT.Service.Helpers.CustomAttributes;
using IGT.Service.Helpers.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace IGT.Service.Helpers
{
    public class RegisterModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}
