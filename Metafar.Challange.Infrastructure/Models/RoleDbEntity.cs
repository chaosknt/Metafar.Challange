using Microsoft.AspNetCore.Identity;

namespace Metafar.Challange.Data.Models
{
    public class RoleDbEntity : IdentityRole<Guid>
    {
        public ICollection<MetafarAccDbEntity> Users { get; set; }
    }
}
