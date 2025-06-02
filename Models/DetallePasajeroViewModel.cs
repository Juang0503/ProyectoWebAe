namespace ProyectoAerolineaWeb.Models
{
    public class DetallePasajeroViewModel
    {
        public int PasajerosId { get; set; }
        public List<DetallePasajero> Detalles { get; set; } = new();

        // Datos del titular
        public string TitularNombre { get; set; }
        public string TitularTelefono { get; set; }
        public string TitularCorreo { get; set; }
    }
}