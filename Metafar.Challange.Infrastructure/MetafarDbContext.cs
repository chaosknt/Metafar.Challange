using Metafar.Challange.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Metafar.Challange.Data
{
    public class MetafarDbContext : DbContext
    {
        public DbSet<MetafarAccDbEntity> MetafarUsers { get; set; }

        public DbSet<AccountMovementDbEntity> AccountMovements { get; set; }

        public MetafarDbContext(DbContextOptions<MetafarDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MetafarAccDbEntity>(entity =>
            {
                entity.Property(e => e.AccountBalance)
                    .HasColumnType("decimal(18, 2)");
            });

            builder.Entity<AccountMovementDbEntity>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)");
            });
        }
    }
}
