using Microsoft.AspNet.Identity.EntityFramework;
using sgs.Models;
using sgs.Models.Domain;
using sgs.Models.Repository;
using sgs.Models.Repository.EntitySql;
using System.Collections.Generic;
using System.Linq;

namespace sgs.Business
{
    public class ListadosService
    {
        IUser userRepo;
        IDistrict districtRepository;
        IMunicipality municipalityRepo;


        public ListadosService() : this(new UserRepository(), new DistrictRepository(), new MunicipalityRepository()) { }
        public ListadosService(IUser _userRepo, IDistrict _districtRepo, IMunicipality _municipalityRepo)
        {
            this.userRepo = _userRepo;
            this.districtRepository = _districtRepo;
            this.municipalityRepo = _municipalityRepo;
        }

        public List<(string nombre, int total, string action)> GetInfoDashboardbyUser(string userId)
        {
            List<(string, int, string)> result = new List<(string, int, string)>();
            var roles = userRepo.GetRolesByUserId(userId);
            var user = userRepo.GetById(userId);
            result.Add(("Mis Registros", userRepo.GetByReferenceKey(userId).Count, "MisRegistros"));

            var contador = userRepo.GetAll().Where(c => c.FatherKey != null).ToList().Count;
            if (roles.Exists(c => c.Name == "Admin") || roles.Exists(c => c.Name == "Sudo"))
            {
                result.Add(("Todos los Registros", contador, "RegistrosTotales"));
                result.Add(("Listar por Distrito", contador, "RegistrosDistritos"));
                result.Add(("Listar por Municipio", contador, "RegistrosMunicipales"));
            }
            if (roles.Exists(c => c.Name == "Distrital"))
            {
                result.Add(("Listar por Distrito", GetValueDashboard("Distrital", user.Addresses.FirstOrDefault().Suburb.Municipality.IdDistrict), "RegistrosDistritos"));
                result.Add(("Listar por Municipio", GetValueDashboard("Distrital", user.Addresses.FirstOrDefault().Suburb.Municipality.IdDistrict), "RegistrosMunicipales"));
            }
            if (roles.Exists(c => c.Name == "Municipal"))
            {
                result.Add(("Listar por Municipio", GetValueDashboard("Municipal", user.Addresses.FirstOrDefault().Suburb.Municipality.IdMunicipality), "RegistrosMunicipales"));
            }
            if (roles.Exists(c => c.Name == "Seccional"))
            {

                result.Add(("Listar por Sección", GetValueDashboard("Seccional", string.IsNullOrEmpty(user.seccional) ? int.Parse(user.Addresses.FirstOrDefault().Seccion) : int.Parse(user.seccional)), "RegistrosSeccionales"));
            }
            return result;
        }
        public int GetValueDashboard(string opcion, int id)
        {
            int result;
            if (opcion == "Distrital")
            {
                return userRepo.GetAll().Where(c => c.FatherKey != null && c.Addresses.FirstOrDefault().Suburb.Municipality.IdDistrict == id).ToList().Count;
            }
            else if (opcion == "Seccional")
            {
                return userRepo.GetAll().Where(c => c.FatherKey != null && int.Parse(c.Addresses.FirstOrDefault().Suburb.Section) == id).ToList().Count;
            }
            else
            {
                return userRepo.GetAll().Where(c => c.FatherKey != null && c.Addresses.FirstOrDefault().Suburb.Municipality.IdMunicipality == id).ToList().Count;
            }
        }
        public List<IdentityRole> GetRoles()
        {

            return userRepo.GetRoles();
        }
        public List<ApplicationUser> RegistrosPorUsuario(string userId)
        {
            var result = userRepo.GetByReferenceKey(userId);
            return result;
        }
        public List<ApplicationUser> TodosLosRegistros()
        {
            var result = userRepo.GetAll();
            return result;
        }
        public List<(string nombre, int total)> GetInfoRolesbyUser(string userId)
        {
            List<(string, int)> result = new List<(string, int)>();

            var registros = userRepo.GetByReferenceKey(userId);
            var roles = GetRoles();
            var group = registros.GroupBy(c => c.Roles.FirstOrDefault().RoleId);

            foreach (var item in group)
            {
                var nombreRole = roles.FirstOrDefault(c => c.Id == item.Key).Name;
                var totalPorRol = item.Count();
                result.Add((nombreRole, totalPorRol));
            }

            return result;
        }

        public List<District> ListarDistritos()
        {
            return districtRepository.GetAll();
        }
        public List<Municipality> ListarMunicipios()
        {
            return municipalityRepo.GetAll();
        }
        public List<Municipality> ListarMunicipiosPorDistrito(int id)
        {
            return municipalityRepo.GetByDistrict(id);
        }
    }
}