using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metafar.Challange.Data.Models
{
    [Table("accountmovements", Schema = "dbo")]
    public class AccountMovementDbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AccountMovementId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("UserId")]
        public virtual MetafarAccDbEntity User { get; set; }
    }
}