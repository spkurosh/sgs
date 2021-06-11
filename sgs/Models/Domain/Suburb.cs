using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgs.Models.Domain
{
    public class Suburb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSuburb { get; set; }

        [Required]
        public string SuburbName { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(5,
        ErrorMessage = "La sección consta de 4 números.")]
        public string Section { get; set; }

        [Required]
        [ForeignKey("Municipality")]
        public int IdMunicipality { get; set; }



        /// <summary>
        /// Navegation Properties
        /// </summary>

        public virtual Municipality Municipality { get; set; }
        public virtual System.Collections.Generic.List<Address> Addresses { get; set; }
    }
}