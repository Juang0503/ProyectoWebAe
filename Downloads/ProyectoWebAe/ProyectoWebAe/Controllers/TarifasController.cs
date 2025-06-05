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
    int? horarioVueloIdVuelta,
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
        var tarifasIda = await _context.Tarifas
            .Where(t => t.HorarioVueloId == horarioVueloIdIda)
            .ToListAsync();

        List<Tarifa> tarifasVuelta = new();
        if (horarioVueloIdVuelta.HasValue)
        {
            tarifasVuelta = await _context.Tarifas
                .Where(t => t.HorarioVueloId == horarioVueloIdVuelta.Value)
                .ToListAsync();
        }

        
        var precioVueloIda = (await _context.HorariosVuelo.FindAsync(horarioVueloIdIda))?.Precio ?? 0;
        decimal precioVueloVuelta = 0;
        if (horarioVueloIdVuelta.HasValue)
            precioVueloVuelta = (await _context.HorariosVuelo.FindAsync(horarioVueloIdVuelta.Value))?.Precio ?? 0;

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
        ViewBag.PrecioVueloIda = precioVueloIda;
        ViewBag.PrecioVueloVuelta = precioVueloVuelta;

        ViewBag.TarifasIda = tarifasIda;
        ViewBag.TarifasVuelta = tarifasVuelta;

        return View();
    }
}