using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryContext: DbContext, IDbContext
    {
        /// <summary>
        /// 2.6	Реализовать репозитории под все сущности кроме референсных 
        /// </summary>
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }       
        public DbSet<Person> Persons { get; set; }
        //public LibraryContext() { }
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {

        }


        /// <summary>
        /// 2.3	Реализовать все связи между таблицами, которые присутствуют в схеме. (использовать при этом только 
        /// fluentAPI)
        /// </summary>
        /// <param name="modelBuilder"></param>
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
