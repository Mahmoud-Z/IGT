using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Interfaces.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Services.UserManagement
{
    public class PrivilegesService : IPrivilegesService
    {
        public IUnitOfWork _unitOfWork { get; set; }

        public PrivilegesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<Privilege>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
