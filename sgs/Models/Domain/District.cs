using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sgs.Models.Domain
{
    public class District
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDistrict { get; set; }

        [Required]
        public string DistrictKey { get; set; }

        [Required]
        public string DistrictName { get; set; }

        [Required]
        [ForeignKey("State")]
        public int IdState { get; set; }


        /// <summary>
        /// Navegation Properties
        /// </summary>
        public virtual State State { get; set; }
        public virtual System.Collections.Generic.List<Municipality> Municipalities { get; set; }
    }
}