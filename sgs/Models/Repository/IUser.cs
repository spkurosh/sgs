using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace sgs.Models.Repository
{
    public interface IUser
    {
        ApplicationUser GetByEmail(string email);
        ApplicationUser GetByVoterKey(string voterKey);
        List<ApplicationUser> GetByReferenceKey(string IdUser);
        ApplicationUser GetById(string id);
        List<IdentityRole> GetRolesByUserId(string id);
        List<ApplicationUser> GetAll();
        List<IdentityRole> GetRoles();
    }
}