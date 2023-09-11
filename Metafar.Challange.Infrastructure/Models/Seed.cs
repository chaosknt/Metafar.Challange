using Metafar.Challange.Common.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Metafar.Challange.Data.Models
{
    public static class Seed
    {

        public static void Init(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MetafarDbContext>();
                var _usermgr = serviceScope.ServiceProvider.GetService<UserManager<MetafarAccDbEntity>>();
                context.Database.EnsureCreated();

                var users = context.MetafarUsers.ToList();
                if (users.Any())
                {
                    return;
                }

                var from = 100000.00m;
                var to = 800000.00m;
                var startAccountValance = RandomNumberGenerator.GenerateRandomDecimal(from, to);

                var user = new MetafarAccDbEntity()
                {
                    CreditCardNumber = CryptographyHelper.ComputeHashSha256("371449635398431"),
                    AccountNumber = "999-999",
                    Pin = CryptographyHelper.ComputeHashSha256("456"),
                    Name = "Mariano Pagani",
                    AccountBalance = startAccountValance,
                    Movements = new List<AccountMovementDbEntity>()
                };

                var user2 = new MetafarAccDbEntity()
                {
                    CreditCardNumber = CryptographyHelper.ComputeHashSha256("471449635398434"),
                    AccountNumber = "444-999",
                    Pin = CryptographyHelper.ComputeHashSha256("654"),
                    Name = "Juan Gomes",
                    AccountBalance = startAccountValance * 2,
                    Movements = new List<AccountMovementDbEntity>()
                };

                context.MetafarUsers.Add(user);
                context.MetafarUsers.Add(user2);
                var movementsUser1 = AddMovements(user);
                var movementsUser2 = AddMovements(user2);

                user.LastExtraction = movementsUser1.Where(x => x.Type == 0).OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
                user2.LastExtraction = movementsUser2.Where(x => x.Type == 0).OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;

                context.AccountMovements.AddRange(movementsUser1);
                context.AccountMovements.AddRange(movementsUser2);

                context.SaveChanges();
            }
        }

        private static List<AccountMovementDbEntity> AddMovements(MetafarAccDbEntity user)
        {
            var movements = new List<AccountMovementDbEntity>();
            for (int i = 0; i < 20; i++)
            {
                var type = RandomNumberGenerator.GenerateRandomNumber(0, 1);
                var value = RandomNumberGenerator.GenerateRandomDecimal(0, user.AccountBalance / 10);

                var newMovement = new AccountMovementDbEntity()
                {
                    AccountMovementId = Guid.NewGuid(),
                    DateTime = DateTime.Now.AddDays((RandomNumberGenerator.GenerateRandomNumber(1, 15) * -1)).AddHours(RandomNumberGenerator.GenerateRandomNumber(1, 15)),
                    Type = type,
                    User = user
                };

                if (type == 1)
                {
                    newMovement.Amount = value;
                    user.AccountBalance += value;
                }
                else
                {
                    newMovement.Amount = value * -1;
                    user.AccountBalance -= value;
                }

                movements.Add(newMovement);
            }

            return movements;
        }
    }
}
