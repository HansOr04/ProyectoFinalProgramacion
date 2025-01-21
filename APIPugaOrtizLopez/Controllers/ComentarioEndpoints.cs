using Microsoft.EntityFrameworkCore;
using APIPugaOrtizLopez.Data;
using APIPugaOrtizLopez.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace APIPugaOrtizLopez.Controllers;

public static class ComentarioEndpoints
{
    public static void MapComentarioEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Comentario").WithTags(nameof(Comentario));

        // GET: Obtener todos los comentarios
        group.MapGet("/", async (BddproyectoFinalContext db) =>
        {
            try
            {
                var comentarios = await db.Comentarios
    .Select(c => new ComentarioResponseDto
    {
        ComentarioId = c.ComentarioId,
        Contenido = c.Contenido,
        FechaCreacion = c.FechaCreacion,
        Departamento = new DepartamentoInfoDto
        {
            DepartamentoId = c.Departamento.DepartamentoId,
            Titulo = c.Departamento.Titulo
        },
        Usuario = new UsuarioResponseDto
        {
            UsuarioId = c.Usuario.UsuarioId,
            Nombre = c.Usuario.Nombre,
            Email = c.Usuario.Email
        }
    })
    .ToListAsync();

                return Results.Ok(comentarios);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al obtener comentarios: {ex.Message}");
            }
        })
        .WithName("GetAllComentarios")
        .WithOpenApi();

        // GET: Obtener comentario por ID
        group.MapGet("/{id}", async (int comentarioid, BddproyectoFinalContext db) =>
        {
            try
            {
                var comentario = await db.Comentarios
    .Include(c => c.Usuario)
    .Include(c => c.Departamento)
    .Select(c => new ComentarioResponseDto
    {
        ComentarioId = c.ComentarioId,
        Contenido = c.Contenido,
        FechaCreacion = c.FechaCreacion,
        Departamento = new DepartamentoInfoDto
        {
            DepartamentoId = c.Departamento.DepartamentoId,
            Titulo = c.Departamento.Titulo
        },
        Usuario = new UsuarioResponseDto
        {
            UsuarioId = c.Usuario.UsuarioId,
            Nombre = c.Usuario.Nombre,
            Email = c.Usuario.Email
        }
    })
    .FirstOrDefaultAsync(c => c.ComentarioId == comentarioid);

                return comentario != null ? Results.Ok(comentario) : Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al obtener el comentario: {ex.Message}");
            }
        })
        .WithName("GetComentarioById")
        .WithOpenApi();

        // POST: Crear comentario
        group.MapPost("/", async (CreateComentarioRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                // Verificar si el usuario existe
                var usuario = await db.Usuarios.FindAsync(request.UsuarioId);
                if (usuario == null)
                {
                    return Results.BadRequest("Usuario no encontrado");
                }

                // Verificar si el departamento existe
                var departamento = await db.Departamentos.FindAsync(request.DepartamentoId);
                if (departamento == null)
                {
                    return Results.BadRequest("Departamento no encontrado");
                }

                var comentario = new Comentario
                {
                    Contenido = request.Contenido,
                    UsuarioId = request.UsuarioId,
                    DepartamentoId = request.DepartamentoId,
                    FechaCreacion = DateTime.Now
                };

                db.Comentarios.Add(comentario);
                await db.SaveChangesAsync();

                var response = new ComentarioResponseDto
                {
                    ComentarioId = comentario.ComentarioId,
                    Contenido = comentario.Contenido,
                    FechaCreacion = comentario.FechaCreacion,
                    Departamento = new DepartamentoInfoDto
                    {
                        DepartamentoId = departamento.DepartamentoId,
                        Titulo = departamento.Titulo
                    },
                    Usuario = new UsuarioResponseDto
                    {
                        UsuarioId = usuario.UsuarioId,
                        Nombre = usuario.Nombre,
                        Email = usuario.Email
                    }
                };

                return Results.Created($"/api/Comentario/{comentario.ComentarioId}", response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al crear el comentario: {ex.Message}");
            }
        })
        .WithName("CreateComentario")
        .WithOpenApi();

        // PUT: Actualizar comentario
        group.MapPut("/{id}", async (int comentarioid, UpdateComentarioRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                var comentario = await db.Comentarios
                    .FirstOrDefaultAsync(c => c.ComentarioId == comentarioid && c.UsuarioId == request.UsuarioId);

                if (comentario == null)
                {
                    return Results.NotFound();
                }

                comentario.Contenido = request.Contenido;
                await db.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al actualizar el comentario: {ex.Message}");
            }
        })
        .WithName("UpdateComentario")
        .WithOpenApi();

        // DELETE: Eliminar comentario
        group.MapDelete("/{id}", async (int comentarioid, int usuarioId, BddproyectoFinalContext db) =>
        {
            try
            {
                var comentario = await db.Comentarios
                    .FirstOrDefaultAsync(c => c.ComentarioId == comentarioid &&
                        (c.UsuarioId == usuarioId || c.Departamento.UsuarioId == usuarioId));

                if (comentario == null)
                {
                    return Results.NotFound();
                }

                db.Comentarios.Remove(comentario);
                await db.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al eliminar el comentario: {ex.Message}");
            }
        })
        .WithName("DeleteComentario")
        .WithOpenApi();
    }
}




