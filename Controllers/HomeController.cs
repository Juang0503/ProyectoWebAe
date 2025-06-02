using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoAerolineaWeb.Models;
using ProyectoAerolineaWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace ProyectoAerolineaWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Página principal con el formulario de búsqueda
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ciudades = await _context.Ciudades.ToListAsync();
            var vuelos = await _context.Vuelos.ToListAsync();

            // Fechas disponibles de ida: fechas de todos los vuelos únicos (ordenadas)
            var fechasDisponiblesIda = vuelos
                .Select(v => v.Fecha.Date)
                .Distinct()
                .OrderBy(f => f)
                .Select(f => f.ToString("yyyy-MM-dd"))
                .ToArray();

            // Fechas disponibles de vuelta: todas las fechas únicas (ordenadas)
            var fechasDisponiblesVuelta = vuelos
                .Select(v => v.Fecha.Date)
                .Distinct()
                .OrderBy(f => f)
                .Select(f => f.ToString("yyyy-MM-dd"))
                .ToArray();

            ViewBag.Ciudades = ciudades;
            ViewBag.Vuelos = vuelos;
            ViewBag.FechasDisponiblesIda = fechasDisponiblesIda;
            ViewBag.FechasDisponiblesVuelta = fechasDisponiblesVuelta;
            return View();
        }

        // Acción para mostrar los horarios según la búsqueda
        [HttpGet]
        public async Task<IActionResult> SeleccionarVuelo(
            int origen, int destino, DateTime fechaIda, DateTime? fechaVuelta,
            int adultos, int ninos, int bebes, int jovenes)
        {
            // Validación de reglas de negocio
            if ((bebes > 0 || ninos > 0) && adultos == 0)
            {
                TempData["Error"] = "Debe haber al menos un adulto para acompañar a bebés o niños.";
                return RedirectToAction("Index");
            }
            if (bebes > adultos)
            {
                TempData["Error"] = "Solo se permite un bebé por adulto.";
                return RedirectToAction("Index");
            }

            int totalPasajeros = adultos + ninos + bebes + jovenes;

            // Buscar vuelos que coincidan con origen, destino, fecha y asientos disponibles
            var vuelos = await _context.Vuelos
                .Include(v => v.CiudadOrigen)
                .Include(v => v.CiudadDestino)
                .Where(v => v.CiudadOrigenId == origen
                            && v.CiudadDestinoId == destino
                            && v.Fecha.Date == fechaIda.Date
                            && v.AsientosDisponibles >= totalPasajeros)
                .ToListAsync();

            // Buscar horarios de esos vuelos
            var horarios = await _context.HorariosVuelo
                .Include(h => h.Vuelo)
                    .ThenInclude(v => v.CiudadOrigen)
                .Include(h => h.Vuelo)
                    .ThenInclude(v => v.CiudadDestino)
                .Where(h => vuelos.Select(v => v.Id).Contains(h.VueloId))
                .ToListAsync();

            // Selector de fechas: precios para -2 a +4 días respecto a la fecha seleccionada
            var fechasPrecios = new List<(DateTime Fecha, decimal Precio)>();
            for (int i = -2; i <= 4; i++)
            {
                var fecha = fechaIda.AddDays(i);
                var vuelosFecha = await _context.Vuelos
                    .Where(v => v.CiudadOrigenId == origen
                                && v.CiudadDestinoId == destino
                                && v.Fecha.Date == fecha.Date
                                && v.AsientosDisponibles >= totalPasajeros)
                    .Select(v => v.Id)
                    .ToListAsync();

                var precio = await _context.HorariosVuelo
                    .Where(h => vuelosFecha.Contains(h.VueloId))
                    .OrderBy(h => h.Precio)
                    .Select(h => h.Precio)
                    .FirstOrDefaultAsync();

                fechasPrecios.Add((fecha, precio));
            }

            ViewBag.Horarios = horarios;
            ViewBag.Origen = _context.Ciudades.FirstOrDefault(c => c.Id == origen)?.Nombre;
            ViewBag.Destino = _context.Ciudades.FirstOrDefault(c => c.Id == destino)?.Nombre;
            ViewBag.FechaIda = fechaIda.ToString("yyyy-MM-dd");
            ViewBag.Adultos = adultos;
            ViewBag.Ninos = ninos;
            ViewBag.Bebes = bebes;
            ViewBag.Jovenes = jovenes;
            ViewBag.TotalPasajeros = totalPasajeros;
            ViewBag.FechasPrecios = fechasPrecios;

            return View("SeleccionarVuelo");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Dashboard()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;

            return View();
        }
    }
}