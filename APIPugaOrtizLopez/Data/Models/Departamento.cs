namespace APIPugaOrtizLopez.Data.Models;

public partial class Departamento
{
    public int DepartamentoId { get; set; }

    public string Titulo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Localizacion { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public int Habitaciones { get; set; }

    public int Baños { get; set; }

    public string LugaresCercanos { get; set; } = null!;

    public string? ImagenUrl { get; set; }

    public int UsuarioId { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Usuario Usuario { get; set; } = null!;
}
