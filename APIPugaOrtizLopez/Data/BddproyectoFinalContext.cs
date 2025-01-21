using System;
using System.Collections.Generic;
using APIPugaOrtizLopez.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPugaOrtizLopez.Data;

public partial class BddproyectoFinalContext : DbContext
{
    public BddproyectoFinalContext()
    {
    }

    public BddproyectoFinalContext(DbContextOptions<BddproyectoFinalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BDDProyectoFinal");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.ToTable("Comentario");

            entity.HasIndex(e => e.DepartamentoId, "IX_Comentario_DepartamentoId");

            entity.HasIndex(e => e.UsuarioId, "IX_Comentario_UsuarioId");

            entity.Property(e => e.Contenido).HasMaxLength(1000);

            entity.HasOne(d => d.Departamento).WithMany(p => p.Comentarios).HasForeignKey(d => d.DepartamentoId);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios).HasForeignKey(d => d.UsuarioId);
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.ToTable("Departamento");

            entity.HasIndex(e => e.UsuarioId, "IX_Departamento_UsuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
