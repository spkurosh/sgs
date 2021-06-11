using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgs.Models.Domain
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAddress { get; set; }


        public string Street { get; set; }

        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string Seccion { get; set; }
        public string PostalCode { get; set; }

        [ForeignKey("User")]
        public string IdUser { get; set; }

        [ForeignKey("Suburb")]
        public int IdSuburb { get; set; }


        /// <summary>
        /// Navegation Properties
        /// </summary>
        /// 

        public virtual ApplicationUser User { get; set; }
        public virtual Suburb Suburb { get; set; }
    }
}