using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class UnitOfWork: IUnitOfWork 
    {
        private readonly IDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;
       

        public UnitOfWork(IDbContext context)
        {
           
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }
            var repository = new GenericRepository<TEntity>(_context);

            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
      
        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
