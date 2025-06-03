using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Data;
using ProyectoAerolineaWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

public class TarifasController : Controller
{
    private readonly ApplicationDbContext _context;

    public TarifasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        int horarioVueloIdIda,
        int? horarioVueloIdVuelta, // Puede ser null si solo es ida
        string origen,
        string destino,
        string fechaIda,
        string fechaVuelta,
        int pasajeros,
        int adultos = 1,
        int ninos = 0,
        int jovenes = 0,
        int bebes = 0)
    {
        // Tarifas para el vuelo de ida
        var tarifasIda = await _context.Tarifas
            .Where(t => t.HorarioVueloId == horarioVueloIdIda)
            .ToListAsync();

        // Tarifas para el vuelo de vuelta (si aplica)
        List<Tarifa> tarifasVuelta = new();
        if (horarioVueloIdVuelta.HasValue)
        {
            tarifasVuelta = await _context.Tarifas
                .Where(t => t.HorarioVueloId == horarioVueloIdVuelta.Value)
                .ToListAsync();
        }

        ViewBag.HorarioVueloIdIda = horarioVueloIdIda;
        ViewBag.HorarioVueloIdVuelta = horarioVueloIdVuelta;
        ViewBag.Origen = origen;
        ViewBag.Destino = destino;
        ViewBag.FechaIda = fechaIda;
        ViewBag.FechaVuelta = fechaVuelta;
        ViewBag.Pasajeros = pasajeros;
        ViewBag.Adultos = adultos;
        ViewBag.Ninos = ninos;
        ViewBag.Jovenes = jovenes;
        ViewBag.Bebes = bebes;

        ViewBag.TarifasIda = tarifasIda;
        ViewBag.TarifasVuelta = tarifasVuelta;

        return View();
    }
}