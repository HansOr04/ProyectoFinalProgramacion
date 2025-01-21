using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/Usuarios/Register.cshtml");
        }

        // POST: Usuario/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Nombre,Email,Password")] Usuario usuario)
        {
            try
            {
                _logger.LogInformation($"Intento de registro para el email: {usuario.Email}");

                if (!ModelState.IsValid)
                {
                    return View("~/Views/Usuarios/Register.cshtml", usuario);
                }

                // Verificar si el email ya existe
                var existingUser = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == usuario.Email.ToLower());

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Este email ya está registrado.");
                    return View("~/Views/Usuarios/Register.cshtml", usuario);
                }

                // Crear nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    Nombre = usuario.Nombre.Trim(),
                    Email = usuario.Email.ToLower().Trim(),
                    PasswordHash = HashPassword(usuario.Password),
                    Comentarios = new List<Comentario>(),
                    Departamentos = new List<Departamento>()
                };

                _context.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Usuario registrado exitosamente: {usuario.Email}");

                TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicie sesión.";
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el registro: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error durante el registro. Por favor, intente nuevamente.");
                return View("~/Views/Usuarios/Register.cshtml", usuario);
            }
        }

        // GET: Usuario/Login
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Usuarios/Login.cshtml");
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "Por favor ingrese email y contraseña");
                    return View("~/Views/Usuarios/Login.cshtml");
                }

                var usuario = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

                if (usuario == null || !VerifyPassword(password, usuario.PasswordHash))
                {
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                    return View("~/Views/Usuarios/Login.cshtml");
                }

                // Guardar información del usuario en la sesión
                HttpContext.Session.SetInt32("UserId", usuario.UsuarioId);
                HttpContext.Session.SetString("UserName", usuario.Nombre);
                HttpContext.Session.SetString("UserEmail", usuario.Email);

                // Configurar claims para la autenticación
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"Usuario {email} ha iniciado sesión exitosamente");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el login: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error durante el inicio de sesión. Por favor, intente nuevamente.");
                return View("~/Views/Usuarios/Login.cshtml");
            }
        }

        // POST: Usuario/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Limpiar la sesión
                HttpContext.Session.Clear();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _logger.LogInformation("Usuario ha cerrado sesión");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el logout: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para hashear la contraseña
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Método para verificar la contraseña
        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var hashedPassword = HashPassword(password);
            return storedHash.Equals(hashedPassword);
        }
    }
}