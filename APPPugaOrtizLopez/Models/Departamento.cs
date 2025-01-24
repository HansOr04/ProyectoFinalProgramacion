using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Models
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


        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }
    }
}
