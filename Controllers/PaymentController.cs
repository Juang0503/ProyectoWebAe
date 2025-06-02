using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoAerolineaWeb.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // aca deben de ir los nombres de los pasajeros, este es un ejemplp
            var pasajeros = new List<PasajeroPagoViewModel>
            {
                new PasajeroPagoViewModel { Nombre = "Juan Pérez", Asiento = "12A", Tarifa = 150.00m, Valor = 150.00m },
                new PasajeroPagoViewModel { Nombre = "Ana Gómez", Asiento = "12B", Tarifa = 150.00m, Valor = 150.00m }
            };

            var model = new PaymentViewModel
            {
                Pasajeros = pasajeros,
                Total = pasajeros.Sum(p => p.Valor)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PaymentViewModel model)
        {
            // Recalcula el total por seguridad
            if (model.Pasajeros != null)
                model.Total = model.Pasajeros.Sum(p => p.Valor);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Aquí iría la lógica real de pago y guardado en base de datos

            TempData["Success"] = "¡Pago realizado correctamente!";
            return RedirectToAction("Index");
        }
    }
}