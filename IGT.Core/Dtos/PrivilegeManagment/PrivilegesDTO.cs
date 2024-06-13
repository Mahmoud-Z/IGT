using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.PrivilegeManagment
{
    public class PrivilegesDTO
    {
        public long PrivilegeId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string BackendURL { get; set; } = null!;

        public PrivilegesDTO(Privilege input) {
            PrivilegeId = input.PrivilegeId;
            Name = input.Name;
            Code = input.Code;
            BackendURL = input.BackendURL;
        }
    }
}
