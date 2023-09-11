using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Data.Models;
using Metafar.Challange.Data.Service.Stores.User;
using Metafar.Challange.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Metafar.Challange.Data.Service.Stores.Movements
{
    public class UserMovementsStore : BaseStore<AccountMovementDbEntity>, IUserMovementsStore
    {
        private readonly IUserStore _userStore;

        public UserMovementsStore(MetafarDbContext context, IUserStore userStore)
           : base(context, context => context.AccountMovements)
        {
            _userStore = userStore;
        }

        public async Task<IQueryable<AccountMovementDbEntity>> GetAllAsync(Expression<Func<AccountMovementDbEntity, bool>> predicate = null)
            => (await base.FilterEntities(predicate)).Include(x => x.User);

        public async Task<AccountMovementDbEntity> Withdrawal(Guid userId, decimal amout)
        {
            var user = (await this._userStore.GetAllAsync(x => x.Id == userId)).FirstOrDefault();
            if(user == null)
            {
                return null;
            }

            user.AccountBalance -= amout;
            user.LastExtraction = DateTime.Now;

            var movement = new AccountMovementDbEntity()
            {
                UserId = userId,
                DateTime = user.LastExtraction.Value,
                Amount = amout,
                Type = AccountMovementsEnum.Withdrawal.AsInt(),
                User = user
            };

            await this.SaveEntity(movement);
            await this._userStore.UpdateUserAsync(user);

            return movement;
        }
    }
}
