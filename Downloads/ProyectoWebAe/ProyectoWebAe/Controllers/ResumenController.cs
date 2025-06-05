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

        // Fechas disponibles para Flatpickr
        var vuelos = _context.Vuelos.ToList();
        var fechasDisponiblesIda = vuelos
            .Select(v => v.Fecha.Date)
            .Distinct()
            .OrderBy(f => f)
            .Select(f => f.ToString("yyyy-MM-dd"))
            .ToArray();
        var fechasDisponiblesVuelta = vuelos
            .Select(v => v.Fecha.Date)
            .Distinct()
            .OrderBy(f => f)
            .Select(f => f.ToString("yyyy-MM-dd"))
            .ToArray();
        ViewData["FechasDisponiblesIda"] = fechasDisponiblesIda;
        ViewData["FechasDisponiblesVuelta"] = fechasDisponiblesVuelta;

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

    [HttpPost]
    public IActionResult Index(
    string origen,
    string destino,
    string fechaIda,
    string fechaVuelta,
    string tarifaNombre,
    decimal tarifaPrecio,
    string tarifaNombreVuelta,
    decimal tarifaPrecioVuelta,
    decimal precioVueloIda,
    decimal precioVueloVuelta,
    int adultos,
    int ninos,
    int jovenes,
    int bebes)
    {
        int pasajeros = adultos + ninos + jovenes + bebes;
        var totalTarifas = (tarifaPrecio + tarifaPrecioVuelta) * pasajeros;
        var totalVuelos = (precioVueloIda + precioVueloVuelta) * pasajeros;
        var totalGeneral = totalTarifas + totalVuelos;

        TempData["TotalGeneral"] = totalGeneral.ToString(System.Globalization.CultureInfo.InvariantCulture);

        // Redirige a la siguiente acción
        return RedirectToAction("DatosPasajero", "Pasajero", new
        {
            adultos,
            ninos,
            jovenes,
            bebes
        });
    }
}