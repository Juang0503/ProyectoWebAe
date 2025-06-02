namespace ProyectoAerolineaWeb.Models
{
    public class Tarifa
    {
        public int Id { get; set; }
        public int HorarioVueloId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Beneficios { get; set; }
        public bool EsRecomendada { get; set; }
    }
}