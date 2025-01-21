namespace APIPugaOrtizLopez.Data.Models
{
    public class UpdateComentarioRequest
    {
        public required string Contenido { get; set; }
        public required int UsuarioId { get; set; }
    }
}
