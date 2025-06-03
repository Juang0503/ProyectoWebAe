using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;

public class ResumenController : Controller
{
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
        decimal precioVueloIda,
        decimal precioVueloVuelta,
        int adultos,
        int ninos,
        int jovenes,
        int bebes)
    {
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
            PrecioVueloIda = precioVueloIda,
            PrecioVueloVuelta = precioVueloVuelta,
            Adultos = adultos,
            Ninos = ninos,
            Jovenes = jovenes,
            Bebes = bebes
        };
        return View("Resumen", model);
    }
}