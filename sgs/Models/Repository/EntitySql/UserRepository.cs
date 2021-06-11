using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sgs.Models.Repository.EntitySql
{
    public class UserRepository : IUser
    {
        ApplicationDbContext context;
        public UserRepository()
        {
            context = context ?? ApplicationDbContext.Create();
        }

        public ApplicationUser GetByEmail(string email)
        {
            var result = context.Users.FirstOrDefault(c => c.Email == email);
            return result;
        }

        public ApplicationUser GetByVoterKey(string voterKey)
        {
            var result = context.Users.FirstOrDefault(c => c.VoterKey == voterKey);
            return result;
        }
        public List<ApplicationUser> GetByReferenceKey(string idUser)
        {
            var result = context.Users.Where(c => c.FatherKey == idUser).Select(c => c).ToList();
            return result;
        }
        public ApplicationUser GetById(string id)
        {
            var result = context.Users.FirstOrDefault(c => c.Id == id);
            return result;
        }
        public List<IdentityRole> GetRolesByUserId(string id)
        {
            var result = context.Roles.Where(c => c.Users.Select(d => d.UserId).Contains(id)).ToList();

            return result;
        }
        public List<IdentityRole> GetRoles()
        {
            var result = context.Roles.ToList();

            return result;
        }
        public List<ApplicationUser> GetAll()
        {
            var result = context.Users.Where(c => c.FatherKey != null).Select(c => c).ToList();

            return result;
        }
    }
}