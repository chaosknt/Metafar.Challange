using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metafar.Challange.Data.Models
{
    public class MetafarAccDbEntity : IdentityUser<Guid>, IDbEntity
    {
        //[Required]
        //[StringLength(70)]
        //public string CreditCardNumber  { get; set; }// username

        //[Required]
        //[StringLength(70)]
        //public string PIN { get; set; } //password

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string AccountNumber { get; set; }

        public decimal AccountBalance { get; set; } = 0;

        public DateTime? LastExtraction { get; set; }

        [NotMapped]
        public virtual ICollection<AccountMovementDbEntity> Movements { get; set; }
    }
}