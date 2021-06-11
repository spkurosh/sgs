using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using sgs.Models;
using System;
using System.Data.SqlClient;
using System.IO;

[assembly: OwinStartupAttribute(typeof(sgs.Startup))]
namespace sgs
{
    public partial class Startup
    {
        string connectionString = "Server=LAPTOP-37U9ESHB;Database=SGSdb;Trusted_Connection=True;";
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            if (!hasData())
            {
                CreateRolesAndUsers();
                InitialDbInfo();
            }
        }

        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Sino existe, Creamos el primer Rol "Sudo" y creamos un Usuario con Rol de "Admin"
            if (!roleManager.RoleExists("Sudo"))
            {
                string userPWD = "123456";

                // Creamos Rol de "Sudo"    
                var role = new IdentityRole();
                role.Name = "Sudo";

                roleManager.Create(role);

                // Aqui creamos un SuperUsuario "Admin" quien mantendrá el sitio web (root)
                var user = new ApplicationUser();
                user.FirstName = "Ciro";
                user.LastName = "Mendoza Eumaña";
                user.VoterKey = "EEEEEE00000021H000";
                user.UserName = "ciro@ejemplo.com";
                user.Email = "ciro@ejemplo.com";
                user.InitialDate = DateTimeOffset.Now;

                var user2 = new ApplicationUser();
                user2.FirstName = "David";
                user2.LastName = "Reyna Moneda";
                user2.VoterKey = "MMMMMM00000021H000";
                user2.UserName = "David@ejemplo.com";
                user2.Email = "David@ejemplo.com";
                user2.InitialDate = DateTimeOffset.Now;


                var chkUser = UserManager.Create(user, userPWD);
                var chkUser2 = UserManager.Create(user2, userPWD);

                // Le damos el rol de "Sudo" a nuestro usuario root
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Sudo");
                }

                if (chkUser2.Succeeded)
                {
                    var result2 = UserManager.AddToRole(user2.Id, "Sudo");
                }

            }

            // Creamos el rol de "Admin"   
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            // Creamos el rol de "Distrital"
            if (!roleManager.RoleExists("Distrital"))
            {
                var role = new IdentityRole();
                role.Name = "Distrital";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Municipal"))
            {
                var role = new IdentityRole();
                role.Name = "Municipal";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Seccional"))
            {
                var role = new IdentityRole();
                role.Name = "Seccional";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Manzana"))
            {
                var role = new IdentityRole();
                role.Name = "Manzana";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Activista"))
            {
                var role = new IdentityRole();
                role.Name = "Activista";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("Promovido"))
            {
                var role = new IdentityRole();
                role.Name = "Promovido";
                roleManager.Create(role);
            }

        }
        private void InitialDbInfo()
        {
            string sqlFileDataState = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/") + "State.sql");
            string sqlFileDataDistrict = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/") + "District.sql");
            string sqlFileDataMunicipality = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/") + "Municipalities.sql");
            string sqlFileDataSuburbs = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/") + "Suburbs.sql");

            string sqlFileViews = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/") + "Views.sql");

            ExecuteSqlCommand(sqlFileDataState);
            ExecuteSqlCommand(sqlFileDataDistrict);
            ExecuteSqlCommand(sqlFileDataMunicipality);
            ExecuteSqlCommand(sqlFileDataSuburbs);
            ExecuteSqlCommand(sqlFileViews);
        }
        private void ExecuteSqlCommand(string sqlFile)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = sqlFile;

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();


                    reader.Close();
                }
                catch (Exception)
                {
                    //ToDo
                }
            }
        }
        private bool hasData()
        {
            bool result = false;
            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    String sql = "SELECT count(*) FROM States";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            result = reader.Read();
                        }
                        connection.Close();
                    }
                }
                return result;
            }
            catch (SqlException)
            {
                return result;
            }
        }
    }
}
