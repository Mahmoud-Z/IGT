using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos.RoleManagment
{
    public class GetAllPrivilegesResponseDTO
    {
        public long PrivilegeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public GetAllPrivilegesResponseDTO(Privilege input)
        {
            this.PrivilegeId = input.PrivilegeId;
            this.Name = input.Name;
            this.Code = input.Code;
        }
    }
}
