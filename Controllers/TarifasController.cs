// Controllers/TarifasController.cs
using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Data;
using ProyectoAerolineaWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

public class TarifasController : Controller
{
    private readonly ApplicationDbContext _context;

    public TarifasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        int horarioVueloId,
        string origen,
        string destino,
        string fechaIda,
        int pasajeros,
        int adultos = 1,
        int ninos = 0,
        int jovenes = 0,
        int bebes = 0)
    {
        var tarifas = await _context.Tarifas
            .Where(t => t.HorarioVueloId == horarioVueloId)
            .ToListAsync();

        ViewBag.HorarioVueloId = horarioVueloId;
        ViewBag.Origen = origen;
        ViewBag.Destino = destino;
        ViewBag.FechaIda = fechaIda;
        ViewBag.Pasajeros = pasajeros;
        ViewBag.Adultos = adultos;
        ViewBag.Ninos = ninos;
        ViewBag.Jovenes = jovenes;
        ViewBag.Bebes = bebes;
        return View(tarifas);
    }
}