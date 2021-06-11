using sgs.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.EntitySql
{
    public class SuburbRepository : ISuburb
    {
        ApplicationDbContext context;
        public SuburbRepository()
        {
            context = context ?? ApplicationDbContext.Create();
        }

        public List<Suburb> GetAllBySection(string section)
        {
            var result = context.Suburb.Select(c => c).Where(c => c.Section == section).ToList();
            return result;
        }
    }
}