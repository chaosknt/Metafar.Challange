using Metafar.Challange.Data.Models;
using System.Linq.Expressions;

namespace Metafar.Challange.Data.Service.Stores.User
{
    public class UserStore : BaseStore<MetafarAccDbEntity>, IUserStore
    {
        public UserStore(MetafarDbContext context)
            : base(context, context => context.MetafarUsers)
        {
        }

        public async Task<IQueryable<MetafarAccDbEntity>> GetAllAsync(Expression<Func<MetafarAccDbEntity, bool>> predicate = null)
            => await base.FilterEntities(predicate);

        public async Task UpdateUserAsync(MetafarAccDbEntity user)
        {
            if(user == null)
            {
                return;
            }

            var userDb = (await this.GetAllAsync(x => x.Id == user.Id)).FirstOrDefault();
            if(userDb == null)
            {
                return;
            }

            userDb.AccessFailedCount = user.AccessFailedCount;
            userDb.AccountBalance = user.AccountBalance;
            userDb.LastExtraction = user.LastExtraction;
            user.RefreshToken = user.RefreshToken;
            user.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

            await this.SaveChangesAsync();
        }
    }
            
}
