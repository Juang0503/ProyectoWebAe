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
    public string FechaVuelta { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Pasajeros { get; set; }

    [BindProperty(SupportsGet = true)]
    public string TarifaNombre { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal TarifaPrecio { get; set; }

    [BindProperty(SupportsGet = true)]
    public string TarifaNombreVuelta { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal TarifaPrecioVuelta { get; set; }

    // Precios de los vuelos seleccionados
    [BindProperty(SupportsGet = true)]
    public decimal PrecioVueloIda { get; set; }
    [BindProperty(SupportsGet = true)]
    public decimal PrecioVueloVuelta { get; set; }

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
        // para carar más datos si lo necesitamos
    }
}