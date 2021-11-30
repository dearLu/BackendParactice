using Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IUnitOfWork : IDisposable 
    {
        public GenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
        public void Save();
        public void Dispose(bool disposing);
        public void Dispose();
    }
}
