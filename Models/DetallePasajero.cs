namespace ProyectoAerolineaWeb.Models
{
    public class DetallePasajero
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Genero { get; set; }
        public string Tipo { get; set; }
        public string Nacionalidad { get; set; }
        public int DiaNacimiento { get; set; }
        public int MesNacimiento { get; set; }
        public int AnioNacimiento { get; set; }
        public string AsientoSeleccionado { get; set; }
    }
}