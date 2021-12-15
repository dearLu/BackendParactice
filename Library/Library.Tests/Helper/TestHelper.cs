using Library.Models;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Services;
using Library.Interfaces;

namespace Library.Tests
{
    public class TestHelper
    {
        private readonly LibraryContext libraryDbContext;
        public TestHelper()
        {
            var builder = new DbContextOptionsBuilder<LibraryContext>();
            builder.UseInMemoryDatabase(databaseName: "LibraryDbInMemory");

            var dbContextOptions = builder.Options;
            libraryDbContext = new LibraryContext(dbContextOptions);
            // Delete existing db before creating a new one
            libraryDbContext.Database.EnsureDeleted();
            libraryDbContext.Database.EnsureCreated();
        }

        public IRepository<Book> GetInMemoryReadRepositoryBook()
        {
            return new GenericRepository<Book>(libraryDbContext);
        }
        public IUnitOfWork GetInMemoryUnitOfWork()
        {
            return new UnitOfWork(libraryDbContext);
        }
    }
}
