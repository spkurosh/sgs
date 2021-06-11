using sgs.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace sgs.Models.Repository.EntitySql
{
    public class DistrictRepository : IDistrict
    {
        ApplicationDbContext context;
        public DistrictRepository()
        {
            context = context ?? ApplicationDbContext.Create();
        }

        public List<District> GetAll()
        {
            return context.Distric.Select(c => c).ToList();
        }
    }
}