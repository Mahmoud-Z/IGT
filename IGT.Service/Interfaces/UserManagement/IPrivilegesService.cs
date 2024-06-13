using IGT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces.UserManagement
{
    public interface IPrivilegesService
    {
        Task<IEnumerable<Privilege>> GetAll();
    }
}
