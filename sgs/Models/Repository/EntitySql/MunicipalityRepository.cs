using sgs.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.EntitySql
{
    public class MunicipalityRepository : IMunicipality
    {
        ApplicationDbContext context;
        public MunicipalityRepository()
        {
            context = context ?? ApplicationDbContext.Create();
        }

        public List<Municipality> GetAll()
        {
            return context.Municipality.Select(c => c).ToList();
        }
        public List<Municipality> GetByDistrict(int id)
        {
            return context.Municipality.Where(c => c.IdDistrict == id).Select(c => c).ToList();
        }
    }
}