using System;
using System.Collections.Generic;

namespace APIPugaOrtizLopez.Data.Models;

public partial class Comentario
{
    public int ComentarioId { get; set; }

    public string Contenido { get; set; } = null!;

    public int UsuarioId { get; set; }

    public int DepartamentoId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Departamento Departamento { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
