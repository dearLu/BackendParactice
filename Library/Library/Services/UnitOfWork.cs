using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class UnitOfWork : IDisposable
    {
        private LibraryContext context = new LibraryContext();
        private GenericRepository<Person> personRepository;
        private GenericRepository<Book> bookRepository;
        private GenericRepository<Genre> genreRepository;
        private GenericRepository<Author> authorRepository;
        public GenericRepository<Person> PersonRepository
        {
            get
            {

                if (this.personRepository == null)
                {
                    this.personRepository = new GenericRepository<Person>(context);
                }
                return personRepository;
            }
        }

        public GenericRepository<Genre> GenreRepository
        {
            get
            {

                if (this.genreRepository == null)
                {
                    this.genreRepository = new GenericRepository<Genre>(context);
                }
                return genreRepository;
            }
        }

        public GenericRepository<Author> AuthorRepository
        {
            get
            {

                if (this.authorRepository == null)
                {
                    this.authorRepository = new GenericRepository<Author>(context);
                }
                return authorRepository;
            }
        }

        public GenericRepository<Book> BookRepository
        {
            get
            {

                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(context);
                }
                return bookRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
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
