using IGT.Core.Enums;
using IGT.Data.DatabaseContext;
using IGT.Data.Models;
using IGT.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Repository.NonGenericRepository
{
    public class SystemStatusCodeRepository : BaseRepository<SystemStatusCode>
    {
        private readonly AkramDbContext _context;
        public SystemStatusCodeRepository(AkramDbContext context) : base(context)
        {
            _context = context;
        }
        public List<SystemStatusCode> GetAllSystemStatusCodes()
        {
            return _context.SystemStatusCodes.ToList();
        }
        public SystemStatusCode? getStatus(string name, string model)
        {
            SystemStatusCode? systemStatusCode = GetAllSystemStatusCodes().FindAll(s => s.Name.Equals(name) && s.Model.Equals(model)).FirstOrDefault();
            return systemStatusCode;
        }
        public SystemStatusCode? getActiveGeneralStatus()
        {
            SystemStatusCode? systemStatusCode = getStatus(SystemStatusCodeEnum.ACTIVE.ToString(), SystemStatusCodeEnum.GENERAL.ToString());
            return systemStatusCode;
        }
        public SystemStatusCode? getDeletedGeneralStatus()
        {
            SystemStatusCode? systemStatusCode = getStatus(SystemStatusCodeEnum.DELETED.ToString(), SystemStatusCodeEnum.GENERAL.ToString());
            return systemStatusCode;
        }
        public SystemStatusCode? getExpiredGeneralStatus()
        {
            SystemStatusCode? systemStatusCode = getStatus(SystemStatusCodeEnum.EXPIRED.ToString(), SystemStatusCodeEnum.GENERAL.ToString());
            return systemStatusCode;
        }
    }
}
