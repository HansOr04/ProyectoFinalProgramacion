using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;

public class BDDProyectoFinal : DbContext
{
    // Constructor que recibe las opciones de configuración de DbContext
    public BDDProyectoFinal(DbContextOptions<BDDProyectoFinal> options)
        : base(options)
    {
    }

    // DbSet para la entidad Usuario
    public DbSet<Usuario> Usuario { get; set; } = default!;

    // DbSet para la entidad Departamento
    public DbSet<Departamento> Departamento { get; set; } = default!;

    // DbSet para la entidad Comentario
    public DbSet<Comentario> Comentario { get; set; } = default!;

    // Método para configurar el modelo de la base de datos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de relaciones y restricciones
        modelBuilder.Entity<Comentario>()
            .HasOne(c => c.Usuario) // Un comentario tiene un usuario
            .WithMany(u => u.Comentarios) // Un usuario puede tener muchos comentarios
            .HasForeignKey(c => c.UsuarioId) // Clave foránea en Comentario
            .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada

        modelBuilder.Entity<Comentario>()
            .HasOne(c => c.Departamento) // Un comentario tiene un departamento
            .WithMany(d => d.Comentarios) // Un departamento puede tener muchos comentarios
            .HasForeignKey(c => c.DepartamentoId) // Clave foránea en Comentario
            .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada
    }
}