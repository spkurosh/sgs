using sgs.Models.Domain;
using System.Collections.Generic;

namespace sgs.Models.Repository
{
    public interface IMunicipality
    {
        List<Municipality> GetAll();
        List<Municipality> GetByDistrict(int id);
    }
}