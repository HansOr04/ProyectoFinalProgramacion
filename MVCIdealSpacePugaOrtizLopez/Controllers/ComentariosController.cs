using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVCIdealSpacePugaOrtizLopez.Models;

namespace MVCIdealSpacePugaOrtizLopez.Controllers
{
    public class ComentariosController : Controller
    {
        private readonly BDDProyectoFinal _context;
        private readonly ILogger<ComentariosController> _logger;

        public ComentariosController(BDDProyectoFinal context, ILogger<ComentariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Comentarios
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accediendo a la lista de comentarios.");
            Debug.WriteLine("Accediendo a la lista de comentarios.");

            var bDDProyectoFinal = _context.Comentario.Include(c => c.Departamento).Include(c => c.Usuario);
            return View(await bDDProyectoFinal.ToListAsync());
        }

        // GET: Comentarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Intento de acceder a detalles de comentario sin ID.");
                Debug.WriteLine("Intento de acceder a detalles de comentario sin ID.");
                return NotFound();
            }

            var comentario = await _context.Comentario
                .Include(c => c.Departamento)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ComentarioId == id);

            if (comentario == null)
            {
                _logger.LogWarning($"No se encontró el comentario con ID {id}.");
                Debug.WriteLine($"No se encontró el comentario con ID {id}.");
                return NotFound();
            }

            _logger.LogInformation($"Mostrando detalles del comentario con ID {id}.");
            Debug.WriteLine($"Mostrando detalles del comentario con ID {id}.");
            return View(comentario);
        }

        // GET: Comentarios/Create
        public IActionResult Create()
        {
            _logger.LogInformation("Accediendo a la vista de creación de comentarios.");
            Debug.WriteLine("Accediendo a la vista de creación de comentarios.");

            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "Ciudad");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Email");
            return View();
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Contenido, int DepartamentoId)
        {
            try
            {
                if (string.IsNullOrEmpty(Contenido))
                {
                    TempData["Error"] = "El contenido del comentario es obligatorio.";
                    _logger.LogWarning("Intento de crear un comentario sin contenido.");
                    return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
                }

                if (!User.Identity.IsAuthenticated)
                {
                    TempData["Error"] = "Debes iniciar sesión para comentar.";
                    _logger.LogWarning("Intento de crear un comentario sin autenticación.");
                    return RedirectToAction("Login", "Usuario");
                }

                // Obtener el nombre del usuario autenticado
                var userName = User.Identity.Name;
                Debug.WriteLine($"Nombre del usuario autenticado: {userName}");
                _logger.LogInformation($"Nombre del usuario autenticado: {userName}");

                if (string.IsNullOrEmpty(userName))
                {
                    TempData["Error"] = "No se pudo obtener el nombre del usuario.";
                    _logger.LogWarning("No se pudo obtener el nombre del usuario autenticado.");
                    return RedirectToAction("Login", "Usuario");
                }

                // Buscar al usuario por su nombre
                var usuarioActual = await _context.Usuario
                    .FirstOrDefaultAsync(u => u.Nombre == userName);

                if (usuarioActual == null)
                {
                    TempData["Error"] = "No se pudo identificar al usuario.";
                    _logger.LogWarning($"No se encontró un usuario con el nombre {userName}.");
                    return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
                }

                // Crear el comentario
                var comentario = new Comentario
                {
                    Contenido = Contenido,
                    UsuarioId = usuarioActual.UsuarioId,
                    DepartamentoId = DepartamentoId,
                    FechaCreacion = DateTime.Now
                };

                _context.Add(comentario);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Comentario publicado exitosamente.";
                _logger.LogInformation($"Comentario creado exitosamente por el usuario {usuarioActual.UsuarioId}.");
                return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error al publicar el comentario.";
                _logger.LogError(ex, "Error al crear un comentario.");
                return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
            }
        }

        // GET: Comentarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Intento de editar comentario sin ID.");
                Debug.WriteLine("Intento de editar comentario sin ID.");
                return NotFound();
            }

            var comentario = await _context.Comentario.FindAsync(id);
            if (comentario == null)
            {
                _logger.LogWarning($"No se encontró el comentario con ID {id} para editar.");
                Debug.WriteLine($"No se encontró el comentario con ID {id} para editar.");
                return NotFound();
            }

            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "Ciudad", comentario.DepartamentoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Email", comentario.UsuarioId);
            _logger.LogInformation($"Accediendo a la edición del comentario con ID {id}.");
            Debug.WriteLine($"Accediendo a la edición del comentario con ID {id}.");
            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComentarioId,Contenido,UsuarioId,DepartamentoId")] Comentario comentario)
        {
            if (id != comentario.ComentarioId)
            {
                _logger.LogWarning($"Intento de editar comentario con ID {id} no coincide con el comentario proporcionado.");
                Debug.WriteLine($"Intento de editar comentario con ID {id} no coincide con el comentario proporcionado.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comentario);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Comentario con ID {id} editado exitosamente.");
                    Debug.WriteLine($"Comentario con ID {id} editado exitosamente.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ComentarioExists(comentario.ComentarioId))
                    {
                        _logger.LogWarning($"No se encontró el comentario con ID {id} para editar.");
                        Debug.WriteLine($"No se encontró el comentario con ID {id} para editar.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, $"Error al editar el comentario con ID {id}.");
                        Debug.WriteLine($"Error al editar el comentario con ID {id}: {ex.Message}");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartamentoId"] = new SelectList(_context.Departamento, "DepartamentoId", "Ciudad", comentario.DepartamentoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "Email", comentario.UsuarioId);
            _logger.LogWarning($"Error de validación al editar el comentario con ID {id}.");
            Debug.WriteLine($"Error de validación al editar el comentario con ID {id}.");
            return View(comentario);
        }

        // GET: Comentarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Intento de eliminar comentario sin ID.");
                Debug.WriteLine("Intento de eliminar comentario sin ID.");
                return NotFound();
            }

            var comentario = await _context.Comentario
                .Include(c => c.Departamento)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ComentarioId == id);

            if (comentario == null)
            {
                _logger.LogWarning($"No se encontró el comentario con ID {id} para eliminar.");
                Debug.WriteLine($"No se encontró el comentario con ID {id} para eliminar.");
                return NotFound();
            }

            _logger.LogInformation($"Accediendo a la eliminación del comentario con ID {id}.");
            Debug.WriteLine($"Accediendo a la eliminación del comentario con ID {id}.");
            return View(comentario);
        }

        // POST: Comentarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);
            if (comentario != null)
            {
                _context.Comentario.Remove(comentario);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Comentario con ID {id} eliminado exitosamente.");
                Debug.WriteLine($"Comentario con ID {id} eliminado exitosamente.");
            }
            else
            {
                _logger.LogWarning($"No se encontró el comentario con ID {id} para eliminar.");
                Debug.WriteLine($"No se encontró el comentario con ID {id} para eliminar.");
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentario.Any(e => e.ComentarioId == id);
        }
    }
}