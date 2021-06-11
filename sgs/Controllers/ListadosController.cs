using Microsoft.AspNet.Identity;
using sgs.Business;
using sgs.Models;
using sgs.Models.Domain;
using sgs.Models.Repository.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sgs.Controllers
{
    public class ListadosController : Controller
    {
        private readonly int _RegistrosPorPagina = 20;
        ListadosService listadosService;
        ComponentsService componentsService;


        private PaginadorGenerico<ApplicationUser> _Paginador;
        public ListadosController() : this(new ListadosService(), new ComponentsService()) { }
        public ListadosController(ListadosService listadosService, ComponentsService componentsService)
        {
            this.listadosService = listadosService;
            this.componentsService = componentsService;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Index(string id, string opcion)
        {
            var userId = User.Identity.GetUserId();
            ViewBag.userId = userId;
            ViewBag.dashboard = listadosService.GetInfoDashboardbyUser(userId);
            switch (opcion)
            {
                case "MisRegistros":
                    return RedirectToAction("MisRegistros", new { id = id, opcion = "MisRegistros" });
                case "RegistrosTotales":
                    return RedirectToAction("MisRegistros", new { id = id, opcion = "RegistrosTotales" });
                case "RegistrosDistritos":
                    return RedirectToAction("ListarDistritos", new { id = id });
                case "RegistrosMunicipales":
                    return RedirectToAction("ListarMunicipios", new { id = id });
                case "RegistrosSeccionales":
                    return RedirectToAction("MisRegistros", new { id = id, opcion = "RegistrosSeccionales" });
                default:
                    return View();
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult MisRegistros(string id, string opcion, string idDistritoSeleccionado, string idMunicipioSeleccionado, int pagina = 1)
        {
            List<ApplicationUser> model;
            int _TotalRegistros = 0;

            ViewBag.roles = listadosService.GetRoles();
            ViewBag.Usuario = componentsService.GetUser(id);
            ViewBag.DatosGenerales = listadosService.GetInfoRolesbyUser(id);
            ViewBag.opcion = opcion;
            ViewBag.id = id;

            switch (opcion)
            {
                case "MisRegistros":
                    model = listadosService.RegistrosPorUsuario(id);
                    break;
                case "RegistrosTotales":
                    if (User.IsInRole("Admin") || User.IsInRole("Sudo"))
                    {
                        model = listadosService.TodosLosRegistros();
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    break;
                case "RegistrosDistritos":
                    int idDistrito;
                    if (idDistritoSeleccionado == null)
                    {
                        idDistrito = componentsService.GetUser(id).Addresses.FirstOrDefault().Suburb.Municipality.IdDistrict;
                    }
                    else
                    {
                        idDistrito = int.Parse(idDistritoSeleccionado);
                    }

                    model = listadosService.TodosLosRegistros().Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == idDistrito)).Select(c => c).ToList();
                    break;
                case "RegistrosMunicipios":
                    int idMunicipio = int.Parse(idMunicipioSeleccionado);

                    model = listadosService.TodosLosRegistros().Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == idMunicipio)).Select(c => c).ToList();
                    break;
                case "RegistrosSeccionales":
                    var usuario = componentsService.GetUser(id);
                    var seccion = String.IsNullOrEmpty(usuario.seccional) ? usuario.Addresses.FirstOrDefault().Seccion : usuario.seccional;
                    model = listadosService.TodosLosRegistros().Where(c => c.Addresses.Exists(d => d.Seccion == seccion)).Select(c => c).ToList();
                    break;
                default:
                    return RedirectToAction("Index");
            }

            //Paginador
            _TotalRegistros = model.Count();
            var registros = model.OrderBy(x => x.LastName)
                                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                                 .Take(_RegistrosPorPagina)
                                                 .ToList();

            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            _Paginador = new PaginadorGenerico<ApplicationUser>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = registros
            };
            return View(_Paginador);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ListarDistritos(string id)
        {
            var roles = listadosService.GetRoles();
            ViewBag.roles = roles;
            if (User.IsInRole("Admin") || User.IsInRole("Sudo"))
            {
                var vmdistritos = listadosService.ListarDistritos();
                var vmregistros = listadosService.TodosLosRegistros();

                List<(int, string, int, int, int, int, int, int)> modelVb = new List<(int, string, int, int, int, int, int, int)>();

                foreach (var item in vmdistritos)
                {
                    var admin = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Admin").FirstOrDefault().Id);
                    var distrital = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Distrital").FirstOrDefault().Id);
                    var municipal = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Municipal").FirstOrDefault().Id);
                    var seccional = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Seccional").FirstOrDefault().Id);
                    var manzana = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Manzana").FirstOrDefault().Id);
                    var activista = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Activista").FirstOrDefault().Id);
                    var promovido = vmregistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Promovido").FirstOrDefault().Id);
                    modelVb.Add((
                            item.IdDistrict,
                            item.DistrictName,
                            admin.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count(),
                            distrital.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count(),
                            municipal.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count(),
                            manzana.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count(),
                            activista.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count(),
                            promovido.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdDistrict == item.IdDistrict)).Select(c => c).Count()
                        ));
                }


                ViewBag.model = modelVb;
            }
            else
            {
                return RedirectToAction("MisRegistros", new { id = id, opcion = "RegistrosDistritos" });
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ListarMunicipios(string id, int pagina = 1)
        {
            var roles = listadosService.GetRoles();
            ViewBag.roles = roles;
            var user = componentsService.GetUser(id);
            if (User.IsInRole("Admin") || User.IsInRole("Sudo") || User.IsInRole("Distrital"))
            {
                List<Municipality> vmMunicipios = new List<Municipality>();
                if (User.IsInRole("Distrital"))
                {

                    vmMunicipios = listadosService.ListarMunicipiosPorDistrito(user.Addresses.FirstOrDefault().Suburb.Municipality.IdDistrict);
                }

                else
                {
                    vmMunicipios = listadosService.ListarMunicipios().OrderBy(c => c.IdDistrict).ToList();
                }
                var vmRegistros = listadosService.TodosLosRegistros();

                List<(int, string, int, int, int, int, int, int, int)> modelVb = new List<(int, string, int, int, int, int, int, int, int)>();

                foreach (var item in vmMunicipios)
                {
                    var admin = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Admin").FirstOrDefault().Id);
                    var distrital = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Distrital").FirstOrDefault().Id);
                    var municipal = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Municipal").FirstOrDefault().Id);
                    var seccional = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Seccional").FirstOrDefault().Id);
                    var manzana = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Manzana").FirstOrDefault().Id);
                    var activista = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Activista").FirstOrDefault().Id);
                    var promovido = vmRegistros.Where(c => c.Roles.FirstOrDefault().RoleId == roles.Where(d => d.Name == "Promovido").FirstOrDefault().Id);
                    modelVb.Add((
                            item.IdMunicipality,
                            $"{item.District.DistrictName.Substring(0, 11).TrimEnd()} - {item.MunicipalityName}",
                            admin.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            distrital.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            municipal.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            seccional.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            manzana.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            activista.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count(),
                            promovido.Where(c => c.Addresses.Exists(d => d.Suburb.Municipality.IdMunicipality == item.IdMunicipality)).Select(c => c).Count()
                        ));
                }

                ViewBag.model = modelVb;
            }
            else if (User.IsInRole("Municipal"))
            {
                var idMunicipio = user.Addresses.FirstOrDefault().Suburb.IdMunicipality;
                return RedirectToAction("MisRegistros", new { id = id, opcion = "RegistrosMunicipios", idMunicipioSeleccionado = idMunicipio });
            }
            else
            {
                return RedirectToAction("MisRegistros", new { id = id, opcion = "RegistrosMunicipios" });
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeStatus(string id)
        {

            string connectionString = "Server=LAPTOP-37U9ESHB;Database=SGSdb;Trusted_Connection=True;";

            string queryString = null;

            var temp = listadosService.TodosLosRegistros().FirstOrDefault(c => c.Id == id);

            if (temp.IsChecked == false)
                queryString = "update AspNetUsers set IsChecked = 1 where id = '" + temp.Id + "'";
            else if (temp.IsChecked == true)
                queryString = "update AspNetUsers set IsChecked = 0 where id = '" + temp.Id + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Close();
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return RedirectToAction("MisRegistros", new { id = temp.FatherKey, opcion = "MisRegistros" });
        }
    }
}