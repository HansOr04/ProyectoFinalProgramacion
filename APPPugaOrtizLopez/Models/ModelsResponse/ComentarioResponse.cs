using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Models.ModelsResponse
{
    public class ComentarioResponse
    {
        public int ComentarioId { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DepartamentoInfo Departamento { get; set; }
        public UserResponse Usuario { get; set; }
    }
}
