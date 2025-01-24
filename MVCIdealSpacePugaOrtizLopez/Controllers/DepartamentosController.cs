using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;
using System.Diagnostics;

namespace MVCIdealSpacePugaOrtizLopez.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {
        private readonly BDDProyectoFinal _context;
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<DepartamentosController> _logger;

        public DepartamentosController(BDDProyectoFinal context, Cloudinary cloudinary, ILogger<DepartamentosController> logger)
        {
            _context = context;
            _cloudinary = cloudinary;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                Debug.WriteLine("Accediendo a la lista de departamentos");
                Console.WriteLine("Accediendo a la lista de departamentos");
                var departamentos = await _context.Departamento
                    .Include(d => d.Usuario)
                    .Include(d => d.Comentarios)
                    .ToListAsync();
                Debug.WriteLine($"Se encontraron {departamentos.Count} departamentos");
                Console.WriteLine($"Se encontraron {departamentos.Count} departamentos");
                return View("~/Views/Departamentos/Index.cshtml", departamentos);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar departamentos: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Error al cargar departamentos: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            Debug.WriteLine($"Accediendo a detalles del departamento ID: {id}");
            Console.WriteLine($"Accediendo a detalles del departamento ID: {id}");

            if (id == null)
            {
                Debug.WriteLine("ID de departamento es nulo");
                Console.WriteLine("ID de departamento es nulo");
                return NotFound();
            }

            try
            {
                var departamento = await _context.Departamento
                    .Include(d => d.Usuario)
                    .Include(d => d.Comentarios)
                        .ThenInclude(c => c.Usuario)
                    .FirstOrDefaultAsync(m => m.DepartamentoId == id);

                if (departamento == null)
                {
                    Debug.WriteLine($"No se encontró el departamento con ID: {id}");
                    Console.WriteLine($"No se encontró el departamento con ID: {id}");
                    return NotFound();
                }

                Debug.WriteLine($"Departamento encontrado: {departamento.Titulo}");
                Console.WriteLine($"Departamento encontrado: {departamento.Titulo}");
                return View("~/Views/Departamentos/Details.cshtml", departamento);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar detalles del departamento: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Error al cargar detalles del departamento: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        public IActionResult Create()
        {
            Debug.WriteLine("Accediendo al formulario de creación de departamento");
            Console.WriteLine("Accediendo al formulario de creación de departamento");
            return View("~/Views/Departamentos/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descripcion,Localizacion,Ciudad,Habitaciones,Baños,LugaresCercanos")] Departamento departamento, IFormFile imagen)
        {
            try
            {
                Debug.WriteLine("Iniciando creación de departamento");
                Console.WriteLine("Iniciando creación de departamento");
                Debug.WriteLine($"Datos recibidos: Título={departamento.Titulo}, Ciudad={departamento.Ciudad}");
                Console.WriteLine($"Datos recibidos: Título={departamento.Titulo}, Ciudad={departamento.Ciudad}");

                // Limpiar cualquier error del ModelState relacionado con Usuario
                ModelState.Remove("Usuario");
                ModelState.Remove("UsuarioId");

                if (!ModelState.IsValid)
                {
                    Debug.WriteLine("ModelState no válido. Errores:");
                    Console.WriteLine("ModelState no válido. Errores:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Debug.WriteLine($"- {error.ErrorMessage}");
                            Console.WriteLine($"Error de validación: {error.ErrorMessage}");
                        }
                    }
                    return View("~/Views/Departamentos/Create.cshtml", departamento);
                }

                // Asignar el usuario después de la validación
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                Debug.WriteLine($"Usuario ID: {userId}");
                Console.WriteLine($"Usuario ID: {userId}");

                departamento.UsuarioId = userId;
                departamento.Comentarios = new List<Comentario>();

                if (imagen != null && imagen.Length > 0)
                {
                    Debug.WriteLine($"Procesando imagen: {imagen.FileName}");
                    Console.WriteLine($"Procesando imagen: {imagen.FileName}");
                    var uploadResult = await UploadImageToCloudinary(imagen);
                    if (uploadResult != null)
                    {
                        departamento.ImagenUrl = uploadResult.SecureUrl.ToString();
                        Debug.WriteLine($"Imagen subida exitosamente: {departamento.ImagenUrl}");
                        Console.WriteLine($"Imagen subida exitosamente: {departamento.ImagenUrl}");
                    }
                }

                _context.Add(departamento);
                await _context.SaveChangesAsync();
                Debug.WriteLine("Departamento creado exitosamente");
                Console.WriteLine("Departamento creado exitosamente");

                TempData["Success"] = "Departamento creado exitosamente.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al crear departamento: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Error al crear departamento: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error al crear el departamento: {ex.Message}");
                return View("~/Views/Departamentos/Create.cshtml", departamento);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Debug.WriteLine($"Accediendo a edición de departamento ID: {id}");
            Console.WriteLine($"Accediendo a edición de departamento ID: {id}");

            if (id == null)
            {
                Debug.WriteLine("ID de departamento es nulo");
                Console.WriteLine("ID de departamento es nulo");
                return NotFound();
            }

            var departamento = await _context.Departamento.FindAsync(id);
            if (departamento == null)
            {
                Debug.WriteLine($"No se encontró el departamento con ID: {id}");
                Console.WriteLine($"No se encontró el departamento con ID: {id}");
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (departamento.UsuarioId != userId)
            {
                Debug.WriteLine($"Usuario {userId} no autorizado para editar departamento {id}");
                Console.WriteLine($"Usuario {userId} no autorizado para editar departamento {id}");
                return Forbid();
            }

            return View("~/Views/Departamentos/Edit.cshtml", departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartamentoId,Titulo,Descripcion,Localizacion,Ciudad,Habitaciones,Baños,LugaresCercanos,ImagenUrl")] Departamento departamento, IFormFile imagen)
        {
            Debug.WriteLine($"Iniciando edición del departamento ID: {id}");
            Console.WriteLine($"Iniciando edición del departamento ID: {id}");

            if (id != departamento.DepartamentoId)
            {
                Debug.WriteLine($"ID no coincide: {id} vs {departamento.DepartamentoId}");
                Console.WriteLine($"ID no coincide: {id} vs {departamento.DepartamentoId}");
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var existingDepartamento = await _context.Departamento.FindAsync(id);

            if (existingDepartamento?.UsuarioId != userId)
            {
                Debug.WriteLine($"Usuario {userId} no autorizado para editar departamento {id}");
                Console.WriteLine($"Usuario {userId} no autorizado para editar departamento {id}");
                return Forbid();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    Debug.WriteLine("ModelState no válido. Errores:");
                    Console.WriteLine("ModelState no válido. Errores:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Debug.WriteLine($"- {error.ErrorMessage}");
                            Console.WriteLine($"Error de validación: {error.ErrorMessage}");
                        }
                    }
                    return View("~/Views/Departamentos/Edit.cshtml", departamento);
                }

                if (imagen != null && imagen.Length > 0)
                {
                    Debug.WriteLine($"Procesando nueva imagen: {imagen.FileName}");
                    Console.WriteLine($"Procesando nueva imagen: {imagen.FileName}");
                    var uploadResult = await UploadImageToCloudinary(imagen);
                    if (uploadResult != null)
                    {
                        departamento.ImagenUrl = uploadResult.SecureUrl.ToString();
                        Debug.WriteLine($"Nueva imagen subida exitosamente: {departamento.ImagenUrl}");
                        Console.WriteLine($"Nueva imagen subida exitosamente: {departamento.ImagenUrl}");
                    }
                }
                else
                {
                    departamento.ImagenUrl = existingDepartamento.ImagenUrl;
                    Debug.WriteLine("Manteniendo imagen existente");
                    Console.WriteLine("Manteniendo imagen existente");
                }

                departamento.UsuarioId = existingDepartamento.UsuarioId;
                _context.Update(departamento);
                await _context.SaveChangesAsync();
                Debug.WriteLine($"Departamento {id} actualizado exitosamente");
                Console.WriteLine($"Departamento {id} actualizado exitosamente");

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DepartamentoExists(departamento.DepartamentoId))
                {
                    Debug.WriteLine($"Error de concurrencia: Departamento {id} no encontrado");
                    Console.WriteLine($"Error de concurrencia: Departamento {id} no encontrado");
                    return NotFound();
                }
                else
                {
                    Debug.WriteLine($"Error de concurrencia al actualizar departamento {id}: {ex.Message}");
                    Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                    Console.WriteLine($"Error de concurrencia al actualizar departamento {id}: {ex.Message}");
                    Console.WriteLine($"StackTrace: {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Debug.WriteLine($"Accediendo a eliminación de departamento ID: {id}");
            Console.WriteLine($"Accediendo a eliminación de departamento ID: {id}");

            if (id == null)
            {
                Debug.WriteLine("ID de departamento es nulo");
                Console.WriteLine("ID de departamento es nulo");
                return NotFound();
            }

            var departamento = await _context.Departamento
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.DepartamentoId == id);

            if (departamento == null)
            {
                Debug.WriteLine($"No se encontró el departamento con ID: {id}");
                Console.WriteLine($"No se encontró el departamento con ID: {id}");
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (departamento.UsuarioId != userId)
            {
                Debug.WriteLine($"Usuario {userId} no autorizado para eliminar departamento {id}");
                Console.WriteLine($"Usuario {userId} no autorizado para eliminar departamento {id}");
                return Forbid();
            }

            return View("~/Views/Departamentos/Delete.cshtml", departamento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Debug.WriteLine($"Iniciando eliminación del departamento ID: {id}");
            Console.WriteLine($"Iniciando eliminación del departamento ID: {id}");

            var departamento = await _context.Departamento
                .Include(d => d.Comentarios)
                .FirstOrDefaultAsync(d => d.DepartamentoId == id);

            if (departamento == null)
            {
                Debug.WriteLine($"No se encontró el departamento con ID: {id}");
                Console.WriteLine($"No se encontró el departamento con ID: {id}");
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (departamento.UsuarioId != userId)
            {
                Debug.WriteLine($"Usuario {userId} no autorizado para eliminar departamento {id}");
                Console.WriteLine($"Usuario {userId} no autorizado para eliminar departamento {id}");
                return Forbid();
            }

            try
            {
                Debug.WriteLine($"Eliminando {departamento.Comentarios.Count} comentarios asociados");
                Console.WriteLine($"Eliminando {departamento.Comentarios.Count} comentarios asociados");
                _context.Comentario.RemoveRange(departamento.Comentarios);
                _context.Departamento.Remove(departamento);
                await _context.SaveChangesAsync();
                Debug.WriteLine($"Departamento {id} y sus comentarios eliminados exitosamente");
                Console.WriteLine($"Departamento {id} y sus comentarios eliminados exitosamente");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar departamento: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Error al eliminar departamento: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        private bool DepartamentoExists(int id)
        {
            return _context.Departamento.Any(e => e.DepartamentoId == id);
        }

        private async Task<ImageUploadResult> UploadImageToCloudinary(IFormFile imagen)
        {
            try
            {
                Debug.WriteLine($"Iniciando carga de imagen: {imagen.FileName}");
                Console.WriteLine($"Iniciando carga de imagen: {imagen.FileName}");

                if (imagen == null || imagen.Length == 0)
                {
                    Debug.WriteLine("Imagen no válida o vacía");
                    Console.WriteLine("Imagen no válida o vacía");
                    return null;
                }

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagen.FileName, imagen.OpenReadStream()),
                    Transformation = new Transformation()
                        .Width(800)
                        .Height(600)
                        .Crop("fill")
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                Debug.WriteLine($"Imagen subida exitosamente a Cloudinary: {result.SecureUrl}");
                Console.WriteLine($"Imagen subida exitosamente a Cloudinary: {result.SecureUrl}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al subir imagen a Cloudinary: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"Error al subir imagen a Cloudinary: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}