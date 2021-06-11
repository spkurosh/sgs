using Newtonsoft.Json;
using sgs.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sgs.Controllers
{
    public class ComponentController : Controller
    {
        ComponentsService componentsService;
        ListadosService listadosService;
        public ComponentController() : this(new ComponentsService(), new ListadosService()) { }
        public ComponentController(ComponentsService componentsService, ListadosService listadosService)
        {
            this.componentsService = componentsService;
            this.listadosService = listadosService;
        }
        [HttpGet]
        [Authorize]
        public JsonResult GetDataSection(string id)
        {
            var result = componentsService.GetDataSection(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        public JsonResult ExistsEmail(string id)
        {
            var result = componentsService.ExistsEmail(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]
        public JsonResult ExistsVoterKey(string id)
        {
            var result = componentsService.ExistsVoterKey(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public int GetGetTotalregistrosPorUsuario(string id)
        {
            return listadosService.RegistrosPorUsuario(id).Count();
        }

        [HttpGet]
        [Authorize(Roles = "Sudo")]
        public ActionResult ResetPwd(string id)
        {
            if (componentsService.ResetPwd(id))
            {
                ViewBag.Success = "Contraseña restablecida con exito";
            }
            else
            {
                ViewBag.Erro = "No se pudo restablecer la contraseña";
            }

            return RedirectToAction("index", "Listados");
        }
        [HttpGet]
        public ActionResult ExportToExcel()
        {
            var registrosexl = new System.Data.DataTable("Registros");
            registrosexl.Columns.Add("Nombre", typeof(string));
            registrosexl.Columns.Add("Apellidos", typeof(string));
            registrosexl.Columns.Add("Elector", typeof(string));
            registrosexl.Columns.Add("Seccion", typeof(int));
            registrosexl.Columns.Add("Municipio", typeof(string));
            registrosexl.Columns.Add("Distrito", typeof(string));
            registrosexl.Columns.Add("Telefono", typeof(string));
            registrosexl.Columns.Add("email", typeof(string));
            registrosexl.Columns.Add("Referencia", typeof(string));
            registrosexl.Columns.Add("Rol", typeof(string));
            registrosexl.Columns.Add("Fecha de Registro", typeof(DateTime));


            var lista = listadosService.TodosLosRegistros().OrderBy(m => m.InitialDate).ToList();
            foreach (var item in lista)
            {
                var father = componentsService.GetUser(item.FatherKey);
                registrosexl.Rows.Add(item.FirstName,
                                        item.LastName,
                                        item.VoterKey,
                                        item.Addresses.FirstOrDefault().Seccion,
                                        item.Addresses.FirstOrDefault().Suburb.Municipality.MunicipalityName,
                                        item.Addresses.FirstOrDefault().Suburb.Municipality.District.DistrictName,
                                        item.PhoneNumber,
                                        item.Email,
                                        $"{father.FirstName} {father.LastName}",
                                        componentsService.GetRole(item.Roles.FirstOrDefault().RoleId),
                                        item.InitialDate.LocalDateTime);

            }

            var grid = new GridView();
            grid.DataSource = registrosexl;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Listado.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index", "Listados");
        }

    }
}