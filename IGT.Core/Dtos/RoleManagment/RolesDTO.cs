using IGT.Core.Dtos.PrivilegeManagment;
using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.RoleManagment
{
    public class RolesDTO
    {
        public string? Name { get; set; }
        public List<PrivilegesDTO>? PrivilegeCodes { get; set; }
        public RolesDTO(Role input)
        {
            this.Name = input.Name;
            PrivilegeCodes = new List<PrivilegesDTO>();
            foreach (var item in input.Privileges)
            {
                PrivilegeCodes.Add(new PrivilegesDTO(item));
            }
        }
    }
}
