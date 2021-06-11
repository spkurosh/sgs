using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}