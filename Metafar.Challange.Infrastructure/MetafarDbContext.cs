using Metafar.Challange.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Metafar.Challange.Data
{
    public class MetafarDbContext : IdentityDbContext<MetafarAccDbEntity, IdentityRole<Guid>, Guid>
    {
        public DbSet<RoleDbEntity> MetafarRoles { get; set; }

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

    public class DbContextFactory : IDesignTimeDbContextFactory<MetafarDbContext>
    {
        public MetafarDbContext CreateDbContext(string[] args)
        {
            var optionBUilder = new DbContextOptionsBuilder<MetafarDbContext>();
            optionBUilder.UseSqlServer("Server=localhost,1435;Initial Catalog=metafar;User ID=sa;Password=M@riano1!.;MultipleActiveResultSets=True");

            return new MetafarDbContext(optionBUilder.Options);
            //https://www.youtube.com/watch?v=0uckSC52oMA&t=223s&ab_channel=ASP.NETMVC
        }
    }
}
