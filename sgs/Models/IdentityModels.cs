using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sgs.Models.Domain;

namespace sgs.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public string FatherKey { get; set; }
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{6}[0-9]{8}[h|m|H|M][0-9]{3}$",
         ErrorMessage = "La Clave de Elector esta mal escrita.")]
        [StringLength(18,
        ErrorMessage = "La Clave de Elector es incorrecta.")]
        [Index(IsUnique = true)]
        public string VoterKey { get; set; }

        public string seccional { get; set; }
        public DateTimeOffset InitialDate { get; set; }
        public DateTimeOffset FinalDate { get; set; }

        public bool IsChecked { get; set; }

        /// <summary>
        /// Navegation Properties
        /// </summary>
        public virtual System.Collections.Generic.List<Address> Addresses { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<District> Distric { get; set; }
        public DbSet<Municipality> Municipality { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Suburb> Suburb { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}