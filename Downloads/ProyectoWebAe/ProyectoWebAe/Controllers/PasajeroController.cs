using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using System.Collections.Generic;
using System;

namespace ProyectoAerolineaWeb.Controllers
{
    public class PasajeroController : Controller
    {
        [HttpGet]
        public IActionResult DatosPasajero(int adultos = 1, int ninos = 0, int jovenes = 0, int bebes = 0)
        {
            
            if (adultos + ninos + jovenes + bebes == 0)
            {
                TempData["Error"] = "Debe haber al menos un pasajero.";
                return RedirectToAction("Index", "Home");
            }

            var detalles = new List<DetallePasajero>();
            for (int i = 0; i < adultos; i++)
                detalles.Add(new DetallePasajero { Tipo = "Adulto" });
            for (int i = 0; i < ninos; i++)
                detalles.Add(new DetallePasajero { Tipo = "Niño" });
            for (int i = 0; i < jovenes; i++)
                detalles.Add(new DetallePasajero { Tipo = "Joven" });
            for (int i = 0; i < bebes; i++)
                detalles.Add(new DetallePasajero { Tipo = "Bebé" });

            var model = new DetallePasajeroViewModel { Detalles = detalles };
            return View(model);
        }

        
        [HttpPost]
        public IActionResult DatosPasajero(DetallePasajeroViewModel model)
        {
            
            for (int i = 0; i < model.Detalles.Count; i++)
            {
                ModelState.Remove($"Detalles[{i}].AsientoSeleccionado");
            }

            
            int edadMinimaAdulto = 18;
            int anioMaximoAdulto = DateTime.Now.Year - edadMinimaAdulto;
            for (int i = 0; i < model.Detalles.Count; i++)
            {
                var pasajero = model.Detalles[i];
                if (pasajero.Tipo == "Adulto" && pasajero.AnioNacimiento > anioMaximoAdulto)
                {
                    ModelState.AddModelError($"Detalles[{i}].AnioNacimiento", "Un adulto debe tener al menos 18 años.");
                }
            }

            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

           
            foreach (var pasajero in model.Detalles)
            {
                try
                {
                    pasajero.FechaNacimiento = new DateTime(
                        pasajero.AnioNacimiento,
                        pasajero.MesNacimiento,
                        pasajero.DiaNacimiento
                    );
                }
                catch
                {
                    ModelState.AddModelError("", $"Fecha de nacimiento inválida para {pasajero.Nombre} {pasajero.Apellido}");
                    return View(model);
                }
            }

            
            TempData["Pasajeros"] = System.Text.Json.JsonSerializer.Serialize(model.Detalles);

            
            return RedirectToAction("SeatMap", "SeatMap", new { vueloId = 1 });
        }
    }
}