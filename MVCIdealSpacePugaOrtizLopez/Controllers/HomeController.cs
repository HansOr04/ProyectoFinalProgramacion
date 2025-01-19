using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdealSpacePugaOrtizLopez.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BDDProyectoFinal _context;

    public HomeController(ILogger<HomeController> logger, BDDProyectoFinal context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var departamentos = await _context.Departamento.ToListAsync();
            return View(departamentos);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al cargar departamentos: {ex.Message}");
            // Devolver una lista vacía en caso de error
            return View(new List<Departamento>());
        }
    }
}