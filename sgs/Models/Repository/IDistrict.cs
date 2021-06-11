using sgs.Models.Domain;
using System.Collections.Generic;

namespace sgs.Models.Repository
{
    public interface IDistrict
    {
        List<District> GetAll();
    }
}