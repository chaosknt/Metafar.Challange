using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metafar.Challange.Data.Models
{
    public class MetafarAccDbEntity : IdentityUser<Guid>, IDbEntity
    {
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

        [NotMapped]
        public virtual ICollection<AccountMovementDbEntity> Movements { get; set; }
    }
}