using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ResumenModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Origen { get; set; }
    [BindProperty(SupportsGet = true)]
    public string Destino { get; set; }
    [BindProperty(SupportsGet = true)]
    public string FechaIda { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Pasajeros { get; set; }
    [BindProperty(SupportsGet = true)]
    public string TarifaNombre { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal TarifaPrecio { get; set; }

    // Agrega estas propiedades para los tipos de pasajero
    [BindProperty(SupportsGet = true)]
    public int Adultos { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Ninos { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Jovenes { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Bebes { get; set; }

    public void OnGet()
    {
        // Aquí puedes cargar más datos si lo necesitas
    }
}