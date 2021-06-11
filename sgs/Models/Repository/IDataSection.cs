using sgs.Models.ViewModel;
using System.Linq;

namespace sgs.Models.Repository
{
    public interface IDataSection
    {
        IQueryable<DataSection> GetDataSection(string id);
    }
}