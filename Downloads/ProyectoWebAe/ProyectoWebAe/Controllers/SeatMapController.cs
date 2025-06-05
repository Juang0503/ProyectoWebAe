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
    public IActionResult SeatMap(int vueloId, int? editIndex = null, int? cantidadPasajeros = null)
    {
        
        TempData["VueloId"] = vueloId;

        
        List<DetallePasajero> pasajeros = new();
        if (TempData["Pasajeros"] != null)
        {
            pasajeros = System.Text.Json.JsonSerializer.Deserialize<List<DetallePasajero>>(
                TempData["Pasajeros"].ToString()
            );
            TempData.Keep("Pasajeros");
        }
        if (TempData["AsientosSeleccionados"] != null)
        {
            TempData.Keep("AsientosSeleccionados");
        }
        if (TempData["TotalGeneral"] != null)
        {
            TempData.Keep("TotalGeneral");
        }

        
        int cantidad = pasajeros.Count;
        if (cantidadPasajeros.HasValue && cantidadPasajeros.Value > cantidad)
        {
            cantidad = cantidadPasajeros.Value;
        }
        while (pasajeros.Count < cantidad)
        {
            pasajeros.Add(new DetallePasajero());
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
                NombreCompleto = $"{p.Nombre} {p.Apellido}",
                AsientoSeleccionado = p.AsientoSeleccionado 
            }).ToList(),
            AsientosDisponibles = asientos.Where(a => a.Estado == "Disponible").Select(a => a.Numero).ToList()
        };

        ViewBag.Asientos = asientos;
        ViewBag.VueloId = vueloId;
        ViewBag.EditIndex = editIndex;

        return View(model);
    }

    [HttpPost]
    public IActionResult SeatMap(SeleccionAsientoViewModel model, int? editIndex = null)
    {
        
        int vueloId = 0;
        if (TempData["VueloId"] != null)
        {
            int.TryParse(TempData["VueloId"].ToString(), out vueloId);
            TempData.Keep("VueloId");
        }
        if (TempData["TotalGeneral"] != null)
        {
            TempData.Keep("TotalGeneral");
        }

        
        List<DetallePasajero> pasajeros = new();
        if (TempData["Pasajeros"] != null)
        {
            pasajeros = System.Text.Json.JsonSerializer.Deserialize<List<DetallePasajero>>(
                TempData["Pasajeros"].ToString()
            );
        }

        
        var asientosSeleccionados = model.Pasajeros
            .Select(p => p.AsientoSeleccionado)
            .Where(a => !string.IsNullOrEmpty(a))
            .ToList();

        if (asientosSeleccionados.Count != asientosSeleccionados.Distinct().Count())
        {
            ModelState.AddModelError("", "No puedes seleccionar el mismo asiento para más de un pasajero.");

            
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

            ViewBag.Asientos = asientos;
            ViewBag.VueloId = vueloId;
            ViewBag.EditIndex = editIndex;

            
            var pasajerosActualizados = model.Pasajeros.Select(p => new DetallePasajero
            {
                Nombre = p.NombreCompleto?.Split(' ').FirstOrDefault() ?? "",
                Apellido = p.NombreCompleto?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                AsientoSeleccionado = p.AsientoSeleccionado
            }).ToList();

            TempData["Pasajeros"] = System.Text.Json.JsonSerializer.Serialize(pasajerosActualizados);
            TempData["AsientosSeleccionados"] = System.Text.Json.JsonSerializer.Serialize(
                pasajerosActualizados.Select(p => p.AsientoSeleccionado).ToList()
            );
            

            
            var viewModel = new SeleccionAsientoViewModel
            {
                Pasajeros = model.Pasajeros,
                AsientosDisponibles = asientos.Where(a => a.Estado == "Disponible").Select(a => a.Numero).ToList()
            };

            return View(viewModel);
        }

        
        if (editIndex.HasValue)
        {
            
            pasajeros[editIndex.Value].AsientoSeleccionado = model.Pasajeros[editIndex.Value].AsientoSeleccionado;
        }
        else
        {
            
            for (int i = 0; i < pasajeros.Count; i++)
            {
                pasajeros[i].AsientoSeleccionado = model.Pasajeros[i].AsientoSeleccionado;
            }
        }

        
        TempData["Pasajeros"] = System.Text.Json.JsonSerializer.Serialize(pasajeros);
        TempData["AsientosSeleccionados"] = System.Text.Json.JsonSerializer.Serialize(
            pasajeros.Select(p => p.AsientoSeleccionado).ToList()
        );
        TempData["VueloId"] = vueloId;

        if (TempData["TotalGeneral"] != null)
        {
            TempData.Keep("TotalGeneral");
        }

       
        return RedirectToAction("Index", "Payment");
    }
}