using System.ComponentModel.DataAnnotations;

namespace MercanciasJyCAPI.Models
{
    public class Cliente
    {
        public int ClienteID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder los 100 caracteres")]
        public string Email { get; set; }
    }
}
