using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Error.Requerido")]
        [Display(Name = "Display.Usuario")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Error.Requerido")]
        [EmailAddress(ErrorMessage = "Error.Email")]
        [Display(Name = "Display.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Error.Requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Display.Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recuérdame")]
        public bool Recuerdame { get; set; }    
    }
}
