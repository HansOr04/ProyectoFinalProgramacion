using Microsoft.EntityFrameworkCore;
using APIPugaOrtizLopez.Data;
using APIPugaOrtizLopez.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace APIPugaOrtizLopez.Controllers;

public static class DepartamentoEndpoints
{
    public static void MapDepartamentoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Departamento").WithTags(nameof(Departamento));

        // GET: Obtener todos los departamentos
        group.MapGet("/", async (BddproyectoFinalContext db) =>
        {
            try
            {
                var departamentos = await db.Departamentos
                    .Select(d => new DepartamentoResponseDto
                    {
                        DepartamentoId = d.DepartamentoId,
                        Titulo = d.Titulo,
                        Descripcion = d.Descripcion,
                        Localizacion = d.Localizacion,
                        Ciudad = d.Ciudad,
                        Habitaciones = d.Habitaciones,
                        Baños = d.Baños,
                        LugaresCercanos = d.LugaresCercanos,
                        ImagenUrl = d.ImagenUrl,
                        Usuario = new UsuarioResponseDto
                        {
                            UsuarioId = d.Usuario.UsuarioId,
                            Nombre = d.Usuario.Nombre,
                            Email = d.Usuario.Email
                        },
                        Comentarios = d.Comentarios.Select(c => new ComentarioResponseDto
                        {
                            ComentarioId = c.ComentarioId,
                            Contenido = c.Contenido,
                            FechaCreacion = c.FechaCreacion,
                            Usuario = new UsuarioResponseDto
                            {
                                UsuarioId = c.Usuario.UsuarioId,
                                Nombre = c.Usuario.Nombre,
                                Email = c.Usuario.Email
                            }
                        }).ToList()
                    })
                    .ToListAsync();

                return Results.Ok(departamentos);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al obtener departamentos: {ex.Message}");
            }
        })
        .WithName("GetAllDepartamentos")
        .WithOpenApi();

        // GET: Obtener departamento por ID
        group.MapGet("/{id}", async (int departamentoid, BddproyectoFinalContext db) =>
        {
            var departamento = await db.Departamentos
                .Select(d => new DepartamentoResponseDto
                {
                    DepartamentoId = d.DepartamentoId,
                    Titulo = d.Titulo,
                    Descripcion = d.Descripcion,
                    Localizacion = d.Localizacion,
                    Ciudad = d.Ciudad,
                    Habitaciones = d.Habitaciones,
                    Baños = d.Baños,
                    LugaresCercanos = d.LugaresCercanos,
                    ImagenUrl = d.ImagenUrl,
                    Usuario = new UsuarioResponseDto
                    {
                        UsuarioId = d.Usuario.UsuarioId,
                        Nombre = d.Usuario.Nombre,
                        Email = d.Usuario.Email
                    },
                    Comentarios = d.Comentarios.Select(c => new ComentarioResponseDto
                    {
                        ComentarioId = c.ComentarioId,
                        Contenido = c.Contenido,
                        FechaCreacion = c.FechaCreacion,
                        Usuario = new UsuarioResponseDto
                        {
                            UsuarioId = c.Usuario.UsuarioId,
                            Nombre = c.Usuario.Nombre,
                            Email = c.Usuario.Email
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(d => d.DepartamentoId == departamentoid);

            return departamento != null ? Results.Ok(departamento) : Results.NotFound();
        })
        .WithName("GetDepartamentoById")
        .WithOpenApi();

        // POST: Crear departamento
        group.MapPost("/", async (CreateDepartamentoRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                var usuario = await db.Usuarios.FindAsync(request.UsuarioId);
                if (usuario == null)
                {
                    return Results.BadRequest("Usuario no encontrado");
                }

                var departamento = new Departamento
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    Localizacion = request.Localizacion,
                    Ciudad = request.Ciudad,
                    Habitaciones = request.Habitaciones,
                    Baños = request.Baños,
                    LugaresCercanos = request.LugaresCercanos,
                    ImagenUrl = request.ImagenUrl,
                    UsuarioId = request.UsuarioId,
                    Comentarios = new List<Comentario>()
                };

                db.Departamentos.Add(departamento);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Departamento/{departamento.DepartamentoId}",
                    new DepartamentoResponseDto
                    {
                        DepartamentoId = departamento.DepartamentoId,
                        Titulo = departamento.Titulo,
                        Descripcion = departamento.Descripcion,
                        Localizacion = departamento.Localizacion,
                        Ciudad = departamento.Ciudad,
                        Habitaciones = departamento.Habitaciones,
                        Baños = departamento.Baños,
                        LugaresCercanos = departamento.LugaresCercanos,
                        ImagenUrl = departamento.ImagenUrl,
                        Usuario = new UsuarioResponseDto
                        {
                            UsuarioId = usuario.UsuarioId,
                            Nombre = usuario.Nombre,
                            Email = usuario.Email
                        },
                        Comentarios = new List<ComentarioResponseDto>()
                    });
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Error al crear departamento: {ex.Message}");
            }
        })
        .WithName("CreateDepartamento")
        .WithOpenApi();

        // PUT: Actualizar departamento
        group.MapPut("/{id}", async (int departamentoid, UpdateDepartamentoRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                var departamento = await db.Departamentos
                    .Include(d => d.Usuario)
                    .FirstOrDefaultAsync(d => d.DepartamentoId == departamentoid && d.UsuarioId == request.UsuarioId);

                if (departamento == null)
                {
                    return Results.NotFound();
                }

                departamento.Titulo = request.Titulo;
                departamento.Descripcion = request.Descripcion;
                departamento.Localizacion = request.Localizacion;
                departamento.Ciudad = request.Ciudad;
                departamento.Habitaciones = request.Habitaciones;
                departamento.Baños = request.Baños;
                departamento.LugaresCercanos = request.LugaresCercanos;
                departamento.ImagenUrl = request.ImagenUrl;

                await db.SaveChangesAsync();
                return Results.Ok();
            }
            catch (Exception)
            {
                return Results.BadRequest("Error al actualizar el departamento");
            }
        })
        .WithName("UpdateDepartamento")
        .WithOpenApi();

        // DELETE: Eliminar departamento
        group.MapDelete("/{id}", async (int departamentoid, int usuarioId, BddproyectoFinalContext db) =>
        {
            try
            {
                var departamento = await db.Departamentos
                    .Include(d => d.Comentarios)
                    .FirstOrDefaultAsync(d => d.DepartamentoId == departamentoid && d.UsuarioId == usuarioId);

                if (departamento == null)
                {
                    return Results.NotFound();
                }

                db.Comentarios.RemoveRange(departamento.Comentarios);
                db.Departamentos.Remove(departamento);
                await db.SaveChangesAsync();

                return Results.Ok();
            }
            catch (Exception)
            {
                return Results.BadRequest("Error al eliminar el departamento");
            }
        })
        .WithName("DeleteDepartamento")
        .WithOpenApi();
    }
}

// DTOs
public class DepartamentoResponseDto
{
    public int DepartamentoId { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string Localizacion { get; set; }
    public string Ciudad { get; set; }
    public int Habitaciones { get; set; }
    public int Baños { get; set; }
    public string LugaresCercanos { get; set; }
    public string? ImagenUrl { get; set; }
    public UsuarioResponseDto Usuario { get; set; }
    public List<ComentarioResponseDto> Comentarios { get; set; }
}

public class UsuarioResponseDto
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
}

public class ComentarioResponseDto
{
    public int ComentarioId { get; set; }
    public string Contenido { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DepartamentoInfoDto Departamento { get; set; } 
    public UsuarioResponseDto Usuario { get; set; }
}
public class DepartamentoInfoDto
{
    public int DepartamentoId { get; set; }
    public string Titulo { get; set; }
}

public class CreateDepartamentoRequest
{
    public required string Titulo { get; set; }
    public required string Descripcion { get; set; }
    public required string Localizacion { get; set; }
    public required string Ciudad { get; set; }
    public required int Habitaciones { get; set; }
    public required int Baños { get; set; }
    public string? LugaresCercanos { get; set; }
    public string? ImagenUrl { get; set; }
    public required int UsuarioId { get; set; }
}

public class UpdateDepartamentoRequest
{
    public required string Titulo { get; set; }
    public required string Descripcion { get; set; }
    public required string Localizacion { get; set; }
    public required string Ciudad { get; set; }
    public required int Habitaciones { get; set; }
    public required int Baños { get; set; }
    public string? LugaresCercanos { get; set; }
    public string? ImagenUrl { get; set; }
    public required int UsuarioId { get; set; }
}

// DTOs de respuesta específicos para búsqueda
public class DepartamentoSearchResponseDto
{
    public int DepartamentoId { get; set; }
    public string Titulo { get; set; }
    public string Ciudad { get; set; }
    public string Descripcion { get; set; }
    public int Habitaciones { get; set; }
    public int Baños { get; set; }
    public string? ImagenUrl { get; set; }
    public double? Precio { get; set; }
}

// DTO para respuesta de listado simple
public class DepartamentoListItemDto
{
    public int DepartamentoId { get; set; }
    public string Titulo { get; set; }
    public string Ciudad { get; set; }
    public string? ImagenUrl { get; set; }
    public int Habitaciones { get; set; }
    public int Baños { get; set; }
}

// DTOs para estadísticas
public class DepartamentoStatsDto
{
    public int TotalDepartamentos { get; set; }
    public int TotalComentarios { get; set; }
    public Dictionary<string, int> DepartamentosPorCiudad { get; set; }
    public Dictionary<int, int> DepartamentosPorHabitaciones { get; set; }
}

// DTOs para filtros de búsqueda
public class DepartamentoFilterDto
{
    public string? Ciudad { get; set; }
    public int? MinHabitaciones { get; set; }
    public int? MaxHabitaciones { get; set; }
    public int? MinBaños { get; set; }
    public int? MaxBaños { get; set; }
    public string? Localizacion { get; set; }
    public bool? TieneImagenes { get; set; }
}

// DTO para respuesta paginada
public class PaginatedResponseDto<T>
{
    public List<T> Items { get; set; }
    public int TotalItems { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public class PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string? SortBy { get; set; }
    public bool IsDescending { get; set; }
}