namespace APIPugaOrtizLopez.Data.Models
{
    public class CreateComentarioRequest
    {
        public required string Contenido { get; set; }
        public required int UsuarioId { get; set; }
        public required int DepartamentoId { get; set; }
    }
}
