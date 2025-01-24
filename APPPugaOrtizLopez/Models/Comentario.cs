using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Models
{
    public class Comentario
    {
        [Key]
        public int ComentarioId { get; set; }

        [Required(ErrorMessage = "El contenido del comentario es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres.")]
        public string Contenido { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Departamento")]
        public int DepartamentoId { get; set; }

        public virtual Departamento Departamento { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
