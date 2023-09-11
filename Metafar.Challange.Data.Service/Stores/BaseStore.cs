using Metafar.Challange.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Metafar.Challange.Data.Service.Stores
{
    public class BaseStore<T> where T : class, IDbEntity
    {
        internal readonly MetafarDbContext context;
        internal readonly DbSet<T> table;

        internal BaseStore(
            MetafarDbContext context,
            Func<MetafarDbContext, DbSet<T>> tableExpression)
        {
            this.context = context;
            this.table = tableExpression(context);
        }

        internal IQueryable<T> TableQueryable { get; set; }

        internal async Task<T?> SaveEntity(T item)
            => (await SaveEntities(item))?.FirstOrDefault();

        internal Task<IEnumerable<T>> SaveEntities(params T[] items)
            => SaveEntities(items.AsEnumerable());

        internal async Task<IEnumerable<T>> SaveEntities(IEnumerable<T> items)
        {
            try
            {
                foreach (var item in items)
                {
                    if (this.context.Entry<T>(item).State == EntityState.Detached)
                    {
                        this.table.Add(item);
                    }
                }

                await this.context.SaveChangesAsync();
                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal async Task<T> RemoveEntity(T item)
        {
            try
            {
                var itemRemoved = this.table.Remove(item).Entity;

                await this.context.SaveChangesAsync();
                return itemRemoved;

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal Task<IQueryable<T>> FilterEntities(Expression<Func<T, bool>> filter = null)
        {
            var queryable = GetQueryable();
            var result = filter != null ? queryable.Where(filter) : queryable;
            return Task.FromResult(result);
        }

        internal async Task<T> GetEntity(Expression<Func<T, bool>> filter = null)
        {
            var queryable = GetQueryable();
            var result = filter != null ? queryable.Where(filter) : queryable;
            return await result.FirstOrDefaultAsync();
        }

        internal IQueryable<T> GetQueryable()
        {
            return TableQueryable ?? table;
        }

        internal async Task SaveChangesAsync()
        {
            if (this.context.ChangeTracker.HasChanges())
            {
                await this.context.SaveChangesAsync();
            }
        }
    }
}
