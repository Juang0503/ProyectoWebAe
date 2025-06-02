using Microsoft.EntityFrameworkCore;
using ProyectoAerolineaWeb.Models;

namespace ProyectoAerolineaWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Vuelo> Vuelos { get; set; }
        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<ConfirmacionReserva> ConfirmacionesReserva { get; set; }
        public DbSet<DetallePasajero> DetallesPasajero { get; set; }
        public DbSet<HorarioVuelo> HorariosVuelo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vuelo>()
                .HasOne(v => v.CiudadOrigen)
                .WithMany()
                .HasForeignKey(v => v.CiudadOrigenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vuelo>()
                .HasOne(v => v.CiudadDestino)
                .WithMany()
                .HasForeignKey(v => v.CiudadDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarifa>()
                .Property(t => t.Precio)
                .HasColumnType("decimal(18,2)");
        }

        public void Seed()
        {
            // 1. Ciudades
            if (!Ciudades.Any())
            {
                Ciudades.AddRange(
                    new Ciudad { Nombre = "Bogotá" },
                    new Ciudad { Nombre = "Cali" },
                    new Ciudad { Nombre = "Manizales" },
                    new Ciudad { Nombre = "Chocó" },
                    new Ciudad { Nombre = "Perú" },
                    new Ciudad { Nombre = "Afganistán" },
                    new Ciudad { Nombre = "China" },
                    new Ciudad { Nombre = "Conchinchina" }
                );
                SaveChanges();
            }

            // 2. Vuelos (solo ida, rutas originales)
            if (!Vuelos.Any())
            {
                var bogota = Ciudades.First(c => c.Nombre == "Bogotá");
                var cali = Ciudades.First(c => c.Nombre == "Cali");
                var manizales = Ciudades.First(c => c.Nombre == "Manizales");
                var choco = Ciudades.First(c => c.Nombre == "Chocó");
                var peru = Ciudades.First(c => c.Nombre == "Perú");
                var afganistan = Ciudades.First(c => c.Nombre == "Afganistán");
                var china = Ciudades.First(c => c.Nombre == "China");
                var conchinchina = Ciudades.First(c => c.Nombre == "Conchinchina");

                var fechas = Enumerable.Range(1, 30)
                    .Select(offset => DateTime.Today.AddDays(offset))
                    .ToList();

                foreach (var fecha in fechas)
                {
                    Vuelos.AddRange(
                        new Vuelo
                        {
                            NumeroVuelo = $"BOG-CLO-{fecha:yyyyMMdd}",
                            CiudadOrigenId = bogota.Id,
                            CiudadDestinoId = cali.Id,
                            Fecha = fecha,
                            AsientosDisponibles = 100,
                            StockMaletas = 50,
                            StockComidas = 100,
                            StockMascotas = 5
                        },
                        new Vuelo
                        {
                            NumeroVuelo = $"MZL-CHO-{fecha:yyyyMMdd}",
                            CiudadOrigenId = manizales.Id,
                            CiudadDestinoId = choco.Id,
                            Fecha = fecha,
                            AsientosDisponibles = 100,
                            StockMaletas = 50,
                            StockComidas = 100,
                            StockMascotas = 5
                        },
                        new Vuelo
                        {
                            NumeroVuelo = $"PER-AFG-{fecha:yyyyMMdd}",
                            CiudadOrigenId = peru.Id,
                            CiudadDestinoId = afganistan.Id,
                            Fecha = fecha,
                            AsientosDisponibles = 100,
                            StockMaletas = 50,
                            StockComidas = 100,
                            StockMascotas = 5
                        },
                        new Vuelo
                        {
                            NumeroVuelo = $"CHN-CNC-{fecha:yyyyMMdd}",
                            CiudadOrigenId = china.Id,
                            CiudadDestinoId = conchinchina.Id,
                            Fecha = fecha,
                            AsientosDisponibles = 100,
                            StockMaletas = 50,
                            StockComidas = 100,
                            StockMascotas = 5
                        }
                    );
                }
                SaveChanges();
            }

            // 3. Horarios (los mismos para todos los vuelos)
            if (!HorariosVuelo.Any())
            {
                foreach (var vuelo in Vuelos.ToList())
                {
                    HorariosVuelo.AddRange(
                        new HorarioVuelo
                        {
                            VueloId = vuelo.Id,
                            HoraSalida = new TimeSpan(6, 40, 0),
                            HoraLlegada = new TimeSpan(13, 10, 0),
                            Precio = 1106115
                        },
                        new HorarioVuelo
                        {
                            VueloId = vuelo.Id,
                            HoraSalida = new TimeSpan(6, 54, 0),
                            HoraLlegada = new TimeSpan(14, 15, 0),
                            Precio = 1328570
                        },
                        new HorarioVuelo
                        {
                            VueloId = vuelo.Id,
                            HoraSalida = new TimeSpan(8, 0, 0),
                            HoraLlegada = new TimeSpan(13, 10, 0),
                            Precio = 1106115
                        }
                    );
                }
                SaveChanges();
            }

            // 4. Tarifas para cada horario de vuelo
            if (!Tarifas.Any())
            {
                var horarios = HorariosVuelo.ToList();
                foreach (var horario in horarios)
                {
                    Tarifas.AddRange(
                        new Tarifa
                        {
                            HorarioVueloId = horario.Id,
                            Nombre = "basic",
                            Descripcion = "Tarifa básica",
                            Precio = horario.Precio + 100000,
                            Beneficios = "1 artículo personal (bolso);Acumula 3 lifemiles por cada USD;No incluye servicios adicionales",
                            EsRecomendada = false
                        },
                        new Tarifa
                        {
                            HorarioVueloId = horario.Id,
                            Nombre = "classic",
                            Descripcion = "Tarifa clásica",
                            Precio = horario.Precio + 200000,
                            Beneficios = "1 artículo personal (bolso);1 equipaje de mano (10 kg);1 equipaje de bodega (23 kg);Check-in en aeropuerto;Asiento Economy incluido;Acumula 6 lifemiles por cada USD;Menú a bordo;Cambios (antes del vuelo)",
                            EsRecomendada = true
                        },
                        new Tarifa
                        {
                            HorarioVueloId = horario.Id,
                            Nombre = "flex",
                            Descripcion = "Tarifa flexible",
                            Precio = horario.Precio + 400000,
                            Beneficios = "1 artículo personal (bolso);1 equipaje de mano (10 kg);1 equipaje de bodega (23 kg);Check-in en aeropuerto;Asiento Plus (sujeto a disponibilidad);Acumula 8 lifemiles por cada USD;Cambios (antes del vuelo);Reembolso (antes del vuelo)",
                            EsRecomendada = false
                        }
                    );
                }
                SaveChanges();
            }
        }
    }
}