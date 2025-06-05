using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ProyectoAerolineaWeb.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var pasajeros = new List<PasajeroPagoViewModel>();
            decimal totalGeneral = 0;
            int vueloId = 0;

            if (TempData["TotalGeneral"] != null)
            {
                decimal.TryParse(TempData["TotalGeneral"].ToString(), out totalGeneral);
                TempData.Keep("TotalGeneral");
            }
            if (TempData["VueloId"] != null)
            {
                int.TryParse(TempData["VueloId"].ToString(), out vueloId);
                TempData.Keep("VueloId");
            }

            if (TempData["Pasajeros"] != null && TempData["AsientosSeleccionados"] != null)
            {
                var detalles = JsonSerializer.Deserialize<List<DetallePasajero>>(TempData["Pasajeros"].ToString());
                var asientos = JsonSerializer.Deserialize<List<string>>(TempData["AsientosSeleccionados"].ToString());

                for (int i = 0; i < detalles.Count; i++)
                {
                    var asiento = asientos.ElementAtOrDefault(i) ?? "";
                    decimal adicional = CalcularValorPorAsiento(asiento);

                    pasajeros.Add(new PasajeroPagoViewModel
                    {
                        Nombre = $"{detalles[i].Nombre} {detalles[i].Apellido}",
                        Asiento = asiento,
                        Valor = adicional
                    });
                }

                TempData.Keep("Pasajeros");
                TempData.Keep("AsientosSeleccionados");
            }

            var total = totalGeneral + pasajeros.Sum(p => p.Valor);

            var model = new PaymentViewModel
            {
                Pasajeros = pasajeros,
                Total = total
            };

            ViewBag.VueloId = vueloId;

            if (TempData["Success"] != null)
            {
                ViewBag.PagoExitoso = TempData["Success"];
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PaymentViewModel model)
        {
            if (model.Pasajeros != null)
            {
                foreach (var pasajero in model.Pasajeros)
                {
                    pasajero.Valor = CalcularValorPorAsiento(pasajero.Asiento);
                }
                model.Total = model.Pasajeros.Sum(p => p.Valor);
            }

            var cardNumber = model.CardNumber?.Replace(" ", "").Replace("-", "");
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 16)
            {
                ModelState.AddModelError("CardNumber", "El número de tarjeta debe tener al menos 16 dígitos.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ViewBag.PagoExitoso = "¡Pago realizado correctamente!";
            return View(model);
        }

        private static decimal CalcularValorPorAsiento(string asiento)
        {
            if (string.IsNullOrEmpty(asiento)) return 0;
            if (!int.TryParse(asiento.Substring(0, asiento.Length - 1), out int fila)) return 0;
            if (fila >= 1 && fila <= 3) return 300_000;
            if (fila >= 4 && fila <= 5) return 100_000;
            if (fila >= 6 && fila <= 8) return 200_000;
            return 0;
        }
    }
}