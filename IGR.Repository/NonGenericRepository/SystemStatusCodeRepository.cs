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
    }
}
