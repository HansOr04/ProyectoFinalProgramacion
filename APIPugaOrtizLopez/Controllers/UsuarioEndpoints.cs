using APIPugaOrtizLopez.Data;
using APIPugaOrtizLopez.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace APIPugaOrtizLopez.Controllers;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Usuario").WithTags(nameof(Usuario));

        // POST: Registro de usuario
        group.MapPost("/register", async Task<Results<Ok<Usuario>, BadRequest<string>>> (RegisterRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                // Verificar si el email ya existe
                if (await db.Usuarios.AnyAsync(u => u.Email == request.Email))
                {
                    return TypedResults.BadRequest("El email ya está registrado.");
                }

                // Crear el hash de la contraseña
                string passwordHash = HashPassword(request.Password);

                // Crear nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Email = request.Email.ToLower(),
                    PasswordHash = passwordHash
                };

                db.Usuarios.Add(usuario);
                await db.SaveChangesAsync();

                // Retornar usuario sin el hash de la contraseña
                usuario.PasswordHash = null;
                return TypedResults.Ok(usuario);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest($"Error en el registro: {ex.Message}");
            }
        })
        .WithName("Register")
        .WithOpenApi();

        // POST: Login de usuario
        group.MapPost("/login", async Task<Results<Ok<LoginResponse>, BadRequest<string>>> (LoginRequest request, BddproyectoFinalContext db) =>
        {
            try
            {
                var usuario = await db.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == request.Email.ToLower());

                if (usuario == null || !VerifyPassword(request.Password, usuario.PasswordHash))
                {
                    return TypedResults.BadRequest("Email o contraseña incorrectos.");
                }

                var response = new LoginResponse
                {
                    UsuarioId = usuario.UsuarioId,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email
                };

                return TypedResults.Ok(response);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest($"Error en el login: {ex.Message}");
            }
        })
        .WithName("Login")
        .WithOpenApi();
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var hashOfInput = HashPassword(password);
        return storedHash == hashOfInput;
    }
}

// Clases para Request/Response
public class RegisterRequest
{
    public required string Nombre { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponse
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
}