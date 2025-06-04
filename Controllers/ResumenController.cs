using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using ProyectoAerolineaWeb.Data;
using System.Linq;

public class ResumenController : Controller
{
    private readonly ApplicationDbContext _context;

    public ResumenController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(
    string origen,
    string destino,
    string fechaIda,
    string fechaVuelta,
    int pasajeros,
    string tarifaNombre,
    decimal tarifaPrecio,
    string tarifaNombreVuelta,
    decimal tarifaPrecioVuelta,
    int horarioVueloIdIda,
    int? horarioVueloIdVuelta,
    int adultos,
    int ninos,
    int jovenes,
    int bebes)
    {
        var horarioIda = _context.HorariosVuelo.FirstOrDefault(h => h.Id == horarioVueloIdIda);
        var horarioVuelta = horarioVueloIdVuelta.HasValue
            ? _context.HorariosVuelo.FirstOrDefault(h => h.Id == horarioVueloIdVuelta.Value)
            : null;

        var model = new ResumenModel
        {
            Origen = origen,
            Destino = destino,
            FechaIda = fechaIda,
            FechaVuelta = fechaVuelta,
            Pasajeros = pasajeros,
            TarifaNombre = tarifaNombre,
            TarifaPrecio = tarifaPrecio,
            TarifaNombreVuelta = tarifaNombreVuelta,
            TarifaPrecioVuelta = tarifaPrecioVuelta,
            PrecioVueloIda = horarioIda?.Precio ?? 0,
            PrecioVueloVuelta = horarioVuelta?.Precio ?? 0,
            Adultos = adultos,
            Ninos = ninos,
            Jovenes = jovenes,
            Bebes = bebes
        };
        return View("Resumen", model);
    }
}