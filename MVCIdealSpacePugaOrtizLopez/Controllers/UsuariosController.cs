using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MVCIdealSpacePugaOrtizLopez.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly BDDProyectoFinal _context;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(BDDProyectoFinal context, ILogger<UsuarioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Usuario/Register
        public IActionResult Register()
        {
            return View("~/Views/Usuarios/Register.cshtml");
        }

        // POST: Usuario/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Nombre,Email,Password")] Usuario usuario)
        {
            try
            {
                // Escribir los valores recibidos
                Console.WriteLine($"Nombre recibido: {usuario.Nombre}");
                Console.WriteLine($"Email recibido: {usuario.Email}");
                Console.WriteLine($"Password recibido: {usuario.Password}");

                // Verificar campos nulos o vacíos
                if (string.IsNullOrEmpty(usuario.Nombre) ||
                    string.IsNullOrEmpty(usuario.Email) ||
                    string.IsNullOrEmpty(usuario.Password))
                {
                    var error = "Todos los campos son obligatorios";
                    Console.WriteLine($"Error de validación: {error}");
                    ModelState.AddModelError("", error);
                    return View("~/Views/Usuarios/Register.cshtml", usuario);
                }

                // Verificar si el email ya existe
                var existingUser = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Email == usuario.Email);

                if (existingUser != null)
                {
                    var error = "Este email ya está registrado.";
                    Console.WriteLine($"Error: {error}");
                    ModelState.AddModelError("Email", error);
                    return View("~/Views/Usuarios/Register.cshtml", usuario);
                }

                // Crear nuevo usuario para la base de datos
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    PasswordHash = HashPassword(usuario.Password),
                    Comentarios = new List<Comentario>()
                };

                // Agregar y guardar en la base de datos
                _context.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                Console.WriteLine("Usuario guardado exitosamente");

                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el registro: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error al registrar el usuario: {ex.Message}");
            }

            return View("~/Views/Usuarios/Register.cshtml", usuario);
        }

        // GET: Usuario/Login
        public IActionResult Login()
        {
            return View("~/Views/Usuarios/Login.cshtml");
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Por favor ingrese email y contraseña");
                    return View("~/Views/Usuarios/Login.cshtml");
                }

                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == email);

                if (usuario == null || !VerifyPassword(password, usuario.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                    return View("~/Views/Usuarios/Login.cshtml");
                }

                // Guardar información del usuario en la sesión
                HttpContext.Session.SetString("UserName", usuario.Nombre);
                HttpContext.Session.SetString("UserEmail", usuario.Email);

                // Configurar la autenticación
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al iniciar sesión: {ex.Message}");
                return View("~/Views/Usuarios/Login.cshtml");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Método para hashear la contraseña
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Método para verificar la contraseña
        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var hashedPassword = HashPassword(password);
            return storedHash == hashedPassword;
        }
    }
}