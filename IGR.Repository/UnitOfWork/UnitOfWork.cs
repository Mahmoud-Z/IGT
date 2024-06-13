using IGT.Data.DatabaseContext;
using IGT.Data.Models;
using IGT.Repository.GenericRepository;
using IGT.Repository.NonGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AkramDbContext _context;
        public SystemStatusCodeRepository SystemStatusCode { get; private set; }

        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        //public IBooksRepository Books { get; private set; }

        public UnitOfWork(AkramDbContext context)
        {
            _context = context;
            SystemStatusCode = new SystemStatusCodeRepository(_context);
        }

        public BaseRepository<T> GetRepository<T>(bool newRepo = false) where T : class//BaseEntity
        {
            Type newType = typeof(T);
            if (_repositories.ContainsKey(newType))
            {
                _repositories.Remove(newType);
            }
            _repositories.Add(newType, new BaseRepository<T>(_context));
            return (BaseRepository<T>)_repositories[newType];
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

