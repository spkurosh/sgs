using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sgs.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "¿Recordar este explorador?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "¿Recordar cuenta?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [Display(Name = "Nombre:")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Apellidos es obligatorio")]
        [Display(Name = "Apellidos:")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "El campo Clave de Elector es obligatorio")]
        [RegularExpression("^[a-zA-Z]{6}[0-9]{8}[h|m|M|H][0-9]{3}$", ErrorMessage = "EL formato de la clave de elector no es válido")]
        [Display(Name = "Clave de Elector:")]
        public string VoterKey { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un Rol")]
        [Display(Name = "Rol:")]
        public string Role { get; set; }

        [Display(Name = "Calle:")]
        public string Street { get; set; }

        [Display(Name = "Número Exterior:")]
        public string NumExt { get; set; }

        [Display(Name = "Número Interiror:")]
        public string NumInt { get; set; }

        [Required(ErrorMessage = "El campo Sección es obligatorio")]
        [Display(Name = "Sección:")]
        public string Seccion { get; set; }

        [Display(Name = "Teléfono:")]
        public string phoneNumber { get; set; }

        [Display(Name = "Colonia:")]
        [Required(ErrorMessage = "El campo Colonia es obligatorio")]
        public string Suburb { get; set; }

        [Display(Name = "Código Postal:")]
        public string PostalCode { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Seccion")]
        public string Seccional { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}
