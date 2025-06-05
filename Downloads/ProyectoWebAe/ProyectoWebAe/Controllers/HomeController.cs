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

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ciudades = await _context.Ciudades.ToListAsync();
            var vuelos = await _context.Vuelos.ToListAsync();

            
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

            ViewBag.Ciudades = ciudades;
            ViewBag.Vuelos = vuelos;
            ViewBag.FechasDisponiblesIda = fechasDisponiblesIda;
            ViewBag.FechasDisponiblesVuelta = fechasDisponiblesVuelta;
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> SeleccionarVuelo(
    int origen, int destino, DateTime fechaIda, DateTime? fechaVuelta,
    int adultos, int ninos, int bebes, int jovenes)
        {

            int totalPasajeros = adultos + ninos + bebes + jovenes;

            
            var vuelosIda = await _context.Vuelos
                .Include(v => v.CiudadOrigen)
                .Include(v => v.CiudadDestino)
                .Where(v => v.CiudadOrigenId == origen
                            && v.CiudadDestinoId == destino
                            && v.Fecha.Date == fechaIda.Date
                            && v.AsientosDisponibles >= totalPasajeros)
                .ToListAsync();

            var horariosIda = await _context.HorariosVuelo
                .Include(h => h.Vuelo)
                    .ThenInclude(v => v.CiudadOrigen)
                .Include(h => h.Vuelo)
                    .ThenInclude(v => v.CiudadDestino)
                .Where(h => vuelosIda.Select(v => v.Id).Contains(h.VueloId))
                .ToListAsync();

            
            List<Vuelo> vuelosVuelta = new();
            List<HorarioVuelo> horariosVuelta = new();
            if (fechaVuelta.HasValue)
            {
                vuelosVuelta = await _context.Vuelos
                    .Include(v => v.CiudadOrigen)
                    .Include(v => v.CiudadDestino)
                    .Where(v => v.CiudadOrigenId == destino
                                && v.CiudadDestinoId == origen
                                && v.Fecha.Date == fechaVuelta.Value.Date
                                && v.AsientosDisponibles >= totalPasajeros)
                    .ToListAsync();

                horariosVuelta = await _context.HorariosVuelo
                    .Include(h => h.Vuelo)
                        .ThenInclude(v => v.CiudadOrigen)
                    .Include(h => h.Vuelo)
                        .ThenInclude(v => v.CiudadDestino)
                    .Where(h => vuelosVuelta.Select(v => v.Id).Contains(h.VueloId))
                    .ToListAsync();
            }

            
            ViewBag.HorariosIda = horariosIda;
            ViewBag.HorariosVuelta = horariosVuelta;
            ViewBag.Origen = _context.Ciudades.FirstOrDefault(c => c.Id == origen)?.Nombre;
            ViewBag.Destino = _context.Ciudades.FirstOrDefault(c => c.Id == destino)?.Nombre;
            ViewBag.FechaIda = fechaIda.ToString("yyyy-MM-dd");
            ViewBag.FechaVuelta = fechaVuelta?.ToString("yyyy-MM-dd");
            ViewBag.Adultos = adultos;
            ViewBag.Ninos = ninos;
            ViewBag.Bebes = bebes;
            ViewBag.Jovenes = jovenes;
            ViewBag.TotalPasajeros = totalPasajeros;

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