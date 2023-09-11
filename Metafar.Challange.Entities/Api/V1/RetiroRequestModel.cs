using System.ComponentModel.DataAnnotations;

namespace Metafar.Challange.Entities.Api.V1
{
    public class RetiroRequestModel : BaseRequestModel
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo Monto debe ser mayor que 0")]
        [Display(Name = "Monto")]
        public decimal Amount { get; set; }
    }
}
