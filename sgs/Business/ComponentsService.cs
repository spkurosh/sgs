using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using sgs.Models;
using sgs.Models.Domain;
using sgs.Models.Repository;
using sgs.Models.Repository.EntitySql;
using sgs.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace sgs.Business
{
    public class ComponentsService
    {
        ISuburb dataSuburbRepo;
        sgs.Models.Repository.IUser userRepo;


        public ComponentsService() : this(new SuburbRepository(), new UserRepository()) { }
        public ComponentsService(ISuburb _dataSuburb, sgs.Models.Repository.IUser _userRepo)
        {
            this.dataSuburbRepo = _dataSuburb;
            this.userRepo = _userRepo;
        }

        public string GetDataSection(string section)
        {
            var result = dataSuburbRepo.GetAllBySection(section);
            var colonias = result.Select(c => new { c.IdSuburb, c.SuburbName, c.PostalCode, c.Section, c.IdMunicipality, c.Municipality.MunicipalityName, c.Municipality.IdDistrict, c.Municipality.District.DistrictName });
            var jsonString = string.Empty;
            if (colonias.Count() > 0)
            {
                //inicializamos el formato json
                jsonString = $"{{";
                //agregamos el objeto de distrito

                jsonString += $"\"distrito\":{{\"id\":{colonias.FirstOrDefault().IdDistrict},\"nombre\":\"{colonias.FirstOrDefault().DistrictName}\"}},";

                jsonString += $"\"municipio\":{{\"id\":{colonias.FirstOrDefault().IdMunicipality},\"nombre\":\"{colonias.FirstOrDefault().MunicipalityName}\"}},";

                //agregamos las colonias
                jsonString += $"\"colonias\":[";
                foreach (var item in colonias)
                {
                    jsonString += $"{{\"id\":{item.IdSuburb},\"nombre\":\"{item.SuburbName}\"}},";
                }
                jsonString = jsonString.Remove(jsonString.Length - 1, 1);
                //cerramos el corchete de colonias
                jsonString += $"],";
                //Agregamos los codigo postales, esto se hace para filtar los repetidos
                jsonString += $"\"codigoPostal\":[";
                foreach (var item in colonias.GroupBy(c => new { c.PostalCode }).Select(c => c.FirstOrDefault()).ToList())
                {
                    jsonString += $"{{\"id\":\"{item.PostalCode}\",\"nombre\":\"{item.PostalCode}\"}},";
                }
                jsonString = jsonString.Remove(jsonString.Length - 1, 1);
                //cerramos el corchete de colonias
                jsonString += $"]";
                //cerramos las llaves de initial state y json
                jsonString += $"}}";
            }

            return jsonString;
        }
        public bool ExistsEmail(string email)
        {
            return userRepo.GetByEmail(email) == null ? false : true;
        }
        public bool ExistsVoterKey(string voterKey)
        {
            return userRepo.GetByVoterKey(voterKey) == null ? false : true;
        }
        public ApplicationUser GetUser(string id)
        {
            return userRepo.GetById(id);
        }
        public string GetRole(string id)
        {
            return userRepo.GetRoles().Where(c => c.Id == id).FirstOrDefault().Name;
        }
        public bool ResetPwd(string id)
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);
                string newPwd = "123456";
                string hashedNewPwd = userManager.PasswordHasher.HashPassword(newPwd);
                ApplicationUser cUser = userManager.FindById(id);
                store.SetPasswordHashAsync(cUser, hashedNewPwd);
                store.UpdateAsync(cUser);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}