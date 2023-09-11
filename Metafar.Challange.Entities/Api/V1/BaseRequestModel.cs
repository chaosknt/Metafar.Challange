using System.ComponentModel.DataAnnotations;

namespace Metafar.Challange.Entities.Api.V1
{
    public class BaseRequestModel
    {
        [Required]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "El número de tarjeta de crédito debe contener 15 caracteres")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número de tarjeta de crédito debe contener solo números.")]
        public string CreditCardNumber { get; set; } = string.Empty;
    }
}
