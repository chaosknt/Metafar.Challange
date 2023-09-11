using Metafar.Challange.Data.Models;
using System.Linq.Expressions;

namespace Metafar.Challange.Data.Service.Stores.User
{
    public interface IUserStore
    {
        Task<IQueryable<MetafarAccDbEntity>> GetAllAsync(Expression<Func<MetafarAccDbEntity, bool>> predicate = null);

        Task UpdateUserAsync(MetafarAccDbEntity user);
    }
}
