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

    public interface IUnitOfWork : IDisposable
    {
        BaseRepository<T> GetRepository<T>(bool newRepo = false) where T : class;
        public SystemStatusCodeRepository SystemStatusCode { get; }
        Task<int> CompleteAsync();
        int Complete();
    }
}