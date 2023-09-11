using System.ComponentModel.DataAnnotations;

namespace Metafar.Challange.Entities.Api.V1
{
    public class LoginRequestModel : BaseRequestModel
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El número PIN debe contener 15 caracteres")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El número PIN debe contener solo números.")]
        public string PIN { get; set; } = string.Empty;
    }
}
