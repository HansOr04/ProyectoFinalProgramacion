using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCIdealSpacePugaOrtizLopez.Models
{
    public class Usuario
    {
        public Usuario()
        {
            Comentarios = new List<Comentario>();
        }

        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [NotMapped]
        public string Password { get; set; }

        [Column("PasswordHash")] // Especifica el nombre de la columna
        public string? PasswordHash { get; set; } // Marca como nullable

        public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}