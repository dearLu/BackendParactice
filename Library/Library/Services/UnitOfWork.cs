using Library.Interfaces;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class UnitOfWork : IUnitOfWork 
    {
        private LibraryContext context = new();
        //private GenericRepository<Person> personRepository;
        //private GenericRepository<Book> bookRepository;
        //private GenericRepository<Genre> genreRepository;
        //private GenericRepository<Author> authorRepository;
        private Dictionary<Type, object> _repositories;
        private bool disposed = false;
        public GenericRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }


            if (hasCustomRepository)
            {
                var customRepo = new GenericRepository<TEntity>(context);
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<TEntity>(context);
            }

            return (GenericRepository<TEntity>)_repositories[type];
        }

        public void Save()
        {
            context.SaveChanges();
        }

        

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
