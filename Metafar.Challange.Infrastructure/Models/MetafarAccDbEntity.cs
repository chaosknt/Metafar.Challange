using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metafar.Challange.Data.Models
{
    [Table("users", Schema = "dbo")]
    public class MetafarAccDbEntity :IDbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string CreditCardNumber { get; set; }

        public string Pin { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string AccountNumber { get; set; }

        public decimal AccountBalance { get; set; } = 0;

        public DateTime? LastExtraction { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        [NotMapped]
        public virtual ICollection<AccountMovementDbEntity> Movements { get; set; }
    }
}