using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class PersonRepository : IRepository<Person>
    {
        private readonly LibraryContext db;

        public PersonRepository()
        {
            this.db = new LibraryContext();
        }

        public IEnumerable<Person> GetList()
        {
            return db.Persons;
        }

        public Person Get(int id)
        {
            return db.Persons.Find(id);
        }

        public void Create(Person person)
        {
            db.Persons.Add(person);
        }

        public void Update(Person person)
        {
            db.Entry(person).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Person person = db.Persons.Find(id);
            if (person != null)
                db.Persons.Remove(person);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
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
