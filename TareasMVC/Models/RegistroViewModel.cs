using System.ComponentModel.DataAnnotations;

namespace TareasMVC.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "el campo debe ser un correo electrónico valido.")]
        public string  Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
