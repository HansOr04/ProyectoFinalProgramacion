using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace MVCIdealSpacePugaOrtizLopez.Controllers
{
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

        // GET: Departamentos
        public async Task<IActionResult> Index()
        {
            return View("~/Views/Departamentos/Index.cshtml", await _context.Departamento.ToListAsync());
        }

        // GET: Departamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamento
                .Include(d => d.Comentarios)
                    .ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.DepartamentoId == id);

            if (departamento == null)
            {
                return NotFound();
            }

            return View("~/Views/Departamentos/Details.cshtml", departamento);
        }

        // GET: Departamentos/Create
        public IActionResult Create()
        {
            return View("~/Views/Departamentos/Create.cshtml");
        }

        // POST: Departamentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartamentoId,Titulo,Descripcion,Localizacion,Ciudad,Habitaciones,Baños,LugaresCercanos")] Departamento departamento, IFormFile imagen)
        {
            try
            {
                // Log de los datos recibidos
                Console.WriteLine($"Titulo recibido: {departamento.Titulo}");
                Console.WriteLine($"Descripción recibida: {departamento.Descripcion}");
                Console.WriteLine($"Localizacion recibida: {departamento.Localizacion}");
                Console.WriteLine($"Ciudad recibida: {departamento.Ciudad}");
                Console.WriteLine($"Habitaciones recibidas: {departamento.Habitaciones}");
                Console.WriteLine($"Baños recibidos: {departamento.Baños}");
                Console.WriteLine($"Lugares cercanos recibidos: {departamento.LugaresCercanos}");
                Console.WriteLine($"Imagen recibida: {(imagen != null ? "Sí" : "No")}");

                // Inicializar la colección de comentarios
                departamento.Comentarios = new List<Comentario>();

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState no es válido. Errores:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"- {error.ErrorMessage}");
                            _logger.LogError($"Error de validación: {error.ErrorMessage}");
                        }
                    }
                    return View("~/Views/Departamentos/Create.cshtml", departamento);
                }

                if (imagen != null && imagen.Length > 0)
                {
                    Console.WriteLine("Procesando imagen...");
                    var uploadResult = await UploadImageToCloudinary(imagen);
                    if (uploadResult != null)
                    {
                        departamento.ImagenUrl = uploadResult.SecureUrl.ToString();
                        Console.WriteLine($"Imagen subida exitosamente: {departamento.ImagenUrl}");
                    }
                }

                _context.Add(departamento);
                await _context.SaveChangesAsync();
                Console.WriteLine("Departamento guardado exitosamente");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear departamento: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                _logger.LogError($"Error al crear departamento: {ex.Message}");
                _logger.LogError($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error al crear el departamento: {ex.Message}");
                return View("~/Views/Departamentos/Create.cshtml", departamento);
            }
        }

        // GET: Departamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo al intentar editar departamento");
                return NotFound();
            }

            var departamento = await _context.Departamento.FindAsync(id);
            if (departamento == null)
            {
                _logger.LogWarning($"No se encontró el departamento con ID: {id}");
                return NotFound();
            }
            return View("~/Views/Departamentos/Edit.cshtml", departamento);
        }

        // POST: Departamentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartamentoId,Titulo,Descripcion,Localizacion,Ciudad,Habitaciones,Baños,LugaresCercanos,ImagenUrl")] Departamento departamento, IFormFile imagen)
        {
            if (id != departamento.DepartamentoId)
            {
                _logger.LogWarning($"ID no coincide: {id} vs {departamento.DepartamentoId}");
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            _logger.LogError($"Error de validación en Edit: {error.ErrorMessage}");
                        }
                    }
                    return View("~/Views/Departamentos/Edit.cshtml", departamento);
                }

                if (imagen != null && imagen.Length > 0)
                {
                    Console.WriteLine("Procesando nueva imagen para edición...");
                    var uploadResult = await UploadImageToCloudinary(imagen);
                    if (uploadResult != null)
                    {
                        departamento.ImagenUrl = uploadResult.SecureUrl.ToString();
                        Console.WriteLine($"Nueva imagen subida exitosamente: {departamento.ImagenUrl}");
                    }
                }

                _context.Update(departamento);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Departamento {id} actualizado exitosamente");

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DepartamentoExists(departamento.DepartamentoId))
                {
                    _logger.LogError($"Departamento {id} no encontrado al editar");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Error de concurrencia al editar departamento {id}: {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al editar departamento {id}: {ex.Message}");
                ModelState.AddModelError("", $"Error al editar el departamento: {ex.Message}");
                return View("~/Views/Departamentos/Edit.cshtml", departamento);
            }
        }

        // GET: Departamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("ID nulo al intentar eliminar departamento");
                return NotFound();
            }

            var departamento = await _context.Departamento
                .FirstOrDefaultAsync(m => m.DepartamentoId == id);
            if (departamento == null)
            {
                _logger.LogWarning($"No se encontró el departamento {id} para eliminar");
                return NotFound();
            }

            return View("~/Views/Departamentos/Delete.cshtml", departamento);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var departamento = await _context.Departamento.FindAsync(id);
                if (departamento != null)
                {
                    _context.Departamento.Remove(departamento);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Departamento {id} eliminado exitosamente");
                }
                else
                {
                    _logger.LogWarning($"No se encontró el departamento {id} para eliminar");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar departamento {id}: {ex.Message}");
                Console.WriteLine($"Error al eliminar departamento: {ex.Message}");
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
                if (imagen == null || imagen.Length == 0)
                {
                    Console.WriteLine("No se recibió ninguna imagen");
                    return null;
                }

                Console.WriteLine($"Subiendo imagen: {imagen.FileName}");
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imagen.FileName, imagen.OpenReadStream())
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                Console.WriteLine($"Imagen subida exitosamente a Cloudinary: {result.SecureUrl}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir imagen a Cloudinary: {ex.Message}");
                _logger.LogError($"Error al subir imagen a Cloudinary: {ex.Message}");
                throw;
            }
        }
    }
}