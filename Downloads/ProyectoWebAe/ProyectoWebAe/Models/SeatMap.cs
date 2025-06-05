namespace ProyectoAerolineaWeb.Models
{
    public class Asiento
    {
        public string Numero { get; set; }
        public string Estado { get; set; }
    }

    public class SeleccionAsientoViewModel
    {
        public List<PasajeroAsiento> Pasajeros { get; set; } = new();
        public List<string> AsientosDisponibles { get; set; } = new();
    }

    public class PasajeroAsiento
    {
        public int PasajeroId { get; set; }
        public string NombreCompleto { get; set; }
        public string AsientoSeleccionado { get; set; }
    }
}