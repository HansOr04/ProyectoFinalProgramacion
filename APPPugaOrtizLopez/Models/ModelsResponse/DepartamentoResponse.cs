using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPPugaOrtizLopez.Models.ModelsResponse
{
    public class DepartamentoResponse
    {
        public int DepartamentoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Localizacion { get; set; }
        public string Ciudad { get; set; }
        public int Habitaciones { get; set; }
        public int Baños { get; set; }
        public string LugaresCercanos { get; set; }
        public string ImagenUrl { get; set; }
        public UserResponse Usuario { get; set; }
        public List<ComentarioResponse> Comentarios { get; set; }
    }
}
