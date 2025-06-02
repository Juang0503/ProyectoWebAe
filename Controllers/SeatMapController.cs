using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using ProyectoAerolineaWeb.Data;
using System.Collections.Generic;
using System.Linq;

public class SeatMapController : Controller
{
    private readonly ApplicationDbContext _context;

    public SeatMapController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult SeatMap(int vueloId)
    {
        // Recuperar pasajeros desde TempData
        List<DetallePasajero> pasajeros = new();
        if (TempData["Pasajeros"] != null)
        {
            pasajeros = System.Text.Json.JsonSerializer.Deserialize<List<DetallePasajero>>(
                TempData["Pasajeros"].ToString()
            );
            TempData.Keep("Pasajeros");
        }

        // Simulación de asientos
        var asientos = new List<Asiento>
        {
            new Asiento { Numero = "1A", Estado = "Disponible" },
            new Asiento { Numero = "1B", Estado = "Ocupado" },
            new Asiento { Numero = "1C", Estado = "Bloqueado" },
            new Asiento { Numero = "2A", Estado = "Disponible" },
            new Asiento { Numero = "2B", Estado = "Ocupado" },
            new Asiento { Numero = "2C", Estado = "Disponible" },
            new Asiento { Numero = "3A", Estado = "Disponible" },
            new Asiento { Numero = "3B", Estado = "Disponible" },
            new Asiento { Numero = "3C", Estado = "Ocupado" },
            new Asiento { Numero = "4A", Estado = "Bloqueado" },
            new Asiento { Numero = "4B", Estado = "Disponible" },
            new Asiento { Numero = "4C", Estado = "Disponible" },
            new Asiento { Numero = "5A", Estado = "Disponible" },
            new Asiento { Numero = "5B", Estado = "Ocupado" },
            new Asiento { Numero = "5C", Estado = "Disponible" },
            new Asiento { Numero = "6A", Estado = "Disponible" },
            new Asiento { Numero = "6B", Estado = "Disponible" },
            new Asiento { Numero = "6C", Estado = "Bloqueado" },
            new Asiento { Numero = "7A", Estado = "Disponible" },
            new Asiento { Numero = "7B", Estado = "Ocupado" },
            new Asiento { Numero = "7C", Estado = "Disponible" },
            new Asiento { Numero = "8A", Estado = "Disponible" },
            new Asiento { Numero = "8B", Estado = "Disponible" },
            new Asiento { Numero = "8C", Estado = "Ocupado" }
        };

        var model = new SeleccionAsientoViewModel
        {
            Pasajeros = pasajeros.Select((p, idx) => new PasajeroAsiento
            {
                PasajeroId = idx + 1,
                NombreCompleto = $"{p.Nombre} {p.Apellido}"
            }).ToList(),
            AsientosDisponibles = asientos.Where(a => a.Estado == "Disponible").Select(a => a.Numero).ToList()
        };

        ViewBag.Asientos = asientos;
        ViewBag.VueloId = vueloId;

        return View(model);
    }

    [HttpPost]
    public IActionResult SeatMap(SeleccionAsientoViewModel model)
    {
        // Recuperar pasajeros desde TempData
        List<DetallePasajero> pasajeros = new();
        if (TempData["Pasajeros"] != null)
        {
            pasajeros = System.Text.Json.JsonSerializer.Deserialize<List<DetallePasajero>>(
                TempData["Pasajeros"].ToString()
            );
        }

        // Asignar asientos seleccionados a cada pasajero
        for (int i = 0; i < pasajeros.Count; i++)
        {
            pasajeros[i].AsientoSeleccionado = model.Pasajeros[i].AsientoSeleccionado;
        }

        // Guardar en la base de datos
        _context.DetallesPasajero.AddRange(pasajeros);
        _context.SaveChanges();

        // Redirigir a una página de confirmación o resumen
        return RedirectToAction("Index", "Payment");
    }
}