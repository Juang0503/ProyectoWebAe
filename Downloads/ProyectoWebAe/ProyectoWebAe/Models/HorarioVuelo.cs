namespace ProyectoAerolineaWeb.Models
{
    public class HorarioVuelo
    {
        public int Id { get; set; }
        public int VueloId { get; set; }
        public Vuelo Vuelo { get; set; } = default!;
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }
        public decimal Precio { get; set; }
    }
}