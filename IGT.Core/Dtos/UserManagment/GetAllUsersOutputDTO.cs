using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.UserManagment
{
    public class GetAllUsersOutputDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public GetAllUsersOutputDTO(User input)
        {
            FirstName = input.FirstName;
            LastName = input.LastName;
            UserName = input.UserName;
            Email = input.Email;
        }
    }
}
