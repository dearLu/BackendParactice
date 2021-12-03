using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IDbContext 
    {
        DbSet<T> Set<T>() where T : class;

        int SaveChanges();

        void Dispose();
        EntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
