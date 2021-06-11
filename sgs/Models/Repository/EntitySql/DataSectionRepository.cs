using sgs.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.EntitySql
{
    public class DataSectionRepository : IDataSection
    {
        ApplicationDbContext context;
        public DataSectionRepository()
        {
            context = context ?? ApplicationDbContext.Create();
        }

        public IQueryable<DataSection> GetDataSection(string id)
        {
            throw new NotImplementedException();
        }
    }
}