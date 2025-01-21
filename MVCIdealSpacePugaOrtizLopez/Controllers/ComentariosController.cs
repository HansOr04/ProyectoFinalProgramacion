using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MVCIdealSpacePugaOrtizLopez.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentario
                .Include(c => c.Departamento)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();
            return View(comentarios);
        }

        // GET: Comentarios/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentario
                .Include(c => c.Departamento)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(m => m.ComentarioId == id);

            if (comentario == null)
            {
                return NotFound();
            }

            return View(comentario);
        }

        // POST: Comentarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(string Contenido, int DepartamentoId)
        {
            try
            {
                if (string.IsNullOrEmpty(Contenido))
                {
                    TempData["Error"] = "El contenido del comentario es obligatorio.";
                    return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
                }

                // Obtener el ID del usuario actual desde las claims
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Verificar si el departamento existe
                var departamento = await _context.Departamento.FindAsync(DepartamentoId);
                if (departamento == null)
                {
                    TempData["Error"] = "El departamento no existe.";
                    return RedirectToAction("Index", "Home");
                }

                var comentario = new Comentario
                {
                    Contenido = Contenido.Trim(),
                    UsuarioId = userId,
                    DepartamentoId = DepartamentoId,
                    FechaCreacion = DateTime.Now
                };

                _context.Add(comentario);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Comentario publicado exitosamente.";
                return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un comentario");
                TempData["Error"] = "Ocurrió un error al publicar el comentario.";
                return RedirectToAction("Details", "Departamentos", new { id = DepartamentoId });
            }
        }

        // GET: Comentarios/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comentario = await _context.Comentario
                .Include(c => c.Departamento)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.ComentarioId == id);

            if (comentario == null)
            {
                return NotFound();
            }

            // Verificar que el usuario actual sea el autor del comentario
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (comentario.UsuarioId != userId)
            {
                return Forbid();
            }

            return View(comentario);
        }

        // POST: Comentarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ComentarioId,Contenido")] Comentario comentario)
        {
            if (id != comentario.ComentarioId)
            {
                return NotFound();
            }

            // Obtener el comentario original incluyendo sus relaciones
            var comentarioOriginal = await _context.Comentario
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.ComentarioId == id);

            if (comentarioOriginal == null)
            {
                return NotFound();
            }

            // Verificar que el usuario actual sea el autor del comentario
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (comentarioOriginal.UsuarioId != userId)
            {
                return Forbid();
            }

            try
            {
                // Actualizar solo el contenido manteniendo los demás datos
                comentarioOriginal.Contenido = comentario.Contenido.Trim();
                _context.Update(comentarioOriginal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Comentario actualizado exitosamente.";
                return RedirectToAction("Details", "Departamentos", new { id = comentarioOriginal.DepartamentoId });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, $"Error al editar el comentario {id}");
                if (!ComentarioExists(comentario.ComentarioId))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // POST: Comentarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var comentario = await _context.Comentario
                .Include(c => c.Departamento)
                .FirstOrDefaultAsync(c => c.ComentarioId == id);

            if (comentario == null)
            {
                return NotFound();
            }

            // Verificar que el usuario actual sea el autor del comentario o el dueño del departamento
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var departamento = await _context.Departamento.FindAsync(comentario.DepartamentoId);

            if (comentario.UsuarioId != userId && departamento?.UsuarioId != userId)
            {
                return Forbid();
            }

            try
            {
                _context.Comentario.Remove(comentario);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Comentario eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el comentario {id}");
                TempData["Error"] = "Ocurrió un error al eliminar el comentario.";
            }

            return RedirectToAction("Details", "Departamentos", new { id = comentario.DepartamentoId });
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentario.Any(e => e.ComentarioId == id);
        }
    }
}