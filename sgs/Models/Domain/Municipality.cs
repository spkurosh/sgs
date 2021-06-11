using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace sgs.Models.Domain
{
    public class Municipality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMunicipality { get; set; }

        [Required]
        public string MunicipalityKey { get; set; }

        [Required]
        public string MunicipalityName { get; set; }

        [Required]
        [ForeignKey("District")]
        public int IdDistrict { get; set; }

        /// <summary>
        /// Navegation Properties
        /// </summary>
        public virtual District District { get; set; }
        public virtual System.Collections.Generic.List<Suburb> Suburbs { get; set; }
    }
}