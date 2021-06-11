using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context = null;
        private ApplicationDbContext Context
        {
            get { return context = context ?? ApplicationDbContext.Create(); }
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }

        }
    }
}