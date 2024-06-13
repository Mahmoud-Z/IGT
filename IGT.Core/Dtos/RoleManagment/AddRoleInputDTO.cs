using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.RoleManagment
{
    public class AddRoleInputDTO
    {
        public string Name { get; set; }
        public List<string>? PrivilegeCodes { get; set; }
    }
}
