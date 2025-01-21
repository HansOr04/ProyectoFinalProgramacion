using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCIdealSpacePugaOrtizLopez.Models
{
    public class Departamento
    {
        public Departamento()
        {
            Comentarios = new List<Comentario>();
        }

        [Key]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La localización es obligatoria")]
        public string Localizacion { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El número de habitaciones es obligatorio")]
        public int Habitaciones { get; set; }

        [Required(ErrorMessage = "El número de baños es obligatorio")]
        public int Baños { get; set; }

        public string LugaresCercanos { get; set; }

        public string? ImagenUrl { get; set; }

        // Quitamos el Required implícito del UsuarioId
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        [ValidateNever] // Agregamos ValidateNever para excluir de la validación
        public virtual Usuario? Usuario { get; set; }

        [ValidateNever] // Agregamos ValidateNever para la colección
        public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}