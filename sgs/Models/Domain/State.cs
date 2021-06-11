using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace sgs.Models.Domain
{
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdState { get; set; }

        [Required]
        public string StateAbbreviation { get; set; }

        [Required]
        public string StateName { get; set; }


        /// <summary>
        /// Navegation Properties
        /// </summary>
        public virtual System.Collections.Generic.List<District> Districts { get; set; }
    }
}