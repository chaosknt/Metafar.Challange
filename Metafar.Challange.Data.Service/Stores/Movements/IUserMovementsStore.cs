using Metafar.Challange.Data.Models;
using System.Linq.Expressions;

namespace Metafar.Challange.Data.Service.Stores.Movements
{
    public interface IUserMovementsStore
    {
        Task<IQueryable<AccountMovementDbEntity>> GetAllAsync(Expression<Func<AccountMovementDbEntity, bool>> predicate = null);

        Task<AccountMovementDbEntity> Withdrawal(Guid userId, decimal amout);
    }
}
