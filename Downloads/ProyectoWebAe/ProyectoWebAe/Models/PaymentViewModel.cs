using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoAerolineaWeb.Models
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "El número de tarjeta es obligatorio.")]
        [CreditCard(ErrorMessage = "El número de tarjeta no es válido.")]
        [Display(Name = "Número de tarjeta")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "El nombre del titular es obligatorio.")]
        [Display(Name = "Nombre del titular")]
        public string CardHolder { get; set; }

        [Required(ErrorMessage = "El mes de expiración es obligatorio.")]
        [Range(1, 12, ErrorMessage = "Mes inválido.")]
        [Display(Name = "Mes de expiración")]
        public int ExpMonth { get; set; }

        [Required(ErrorMessage = "El año de expiración es obligatorio.")]
        [Range(2024, 2100, ErrorMessage = "Año inválido.")]
        [Display(Name = "Año de expiración")]
        public int ExpYear { get; set; }

        [Required(ErrorMessage = "El código CVV es obligatorio.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV inválido.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        public List<PasajeroPagoViewModel> Pasajeros { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class PasajeroPagoViewModel
    {
        public string Nombre { get; set; }
        public string Asiento { get; set; }
        public decimal Valor { get; set; }
    }
}