using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;

public class ResumenController : Controller
{
    [HttpGet]
    public IActionResult Index(string origen, string destino, string fechaIda, int pasajeros, string tarifaNombre, decimal tarifaPrecio, int adultos, int ninos, int jovenes, int bebes)
    {
        var model = new ResumenModel
        {
            Origen = origen,
            Destino = destino,
            FechaIda = fechaIda,
            Pasajeros = pasajeros,
            TarifaNombre = tarifaNombre,
            TarifaPrecio = tarifaPrecio,
            Adultos = adultos,
            Ninos = ninos,
            Jovenes = jovenes,
            Bebes = bebes
        };
        return View("Resumen", model);
    }
}