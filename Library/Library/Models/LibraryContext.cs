using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryContext: DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        
        public DbSet<Person> Persons { get; set; }
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
           // Database.EnsureCreated();

        }

        public LibraryContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Book>()
            .HasOne(p => p.Author)
            .WithMany(p => p.Books)
            .HasForeignKey(s => s.AuthorId);


            modelBuilder.Entity<Book>()
                .HasMany(t => t.Genres)
                .WithMany(t => t.Books);

            modelBuilder.Entity<Book>()
                .HasMany(t => t.Persons)
                .WithMany(t => t.Books);

            base.OnModelCreating(modelBuilder);
        }
    }
}
