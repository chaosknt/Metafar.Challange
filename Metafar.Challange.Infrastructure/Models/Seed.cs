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
                try
                {
                    var context = serviceScope.ServiceProvider.GetService<MetafarDbContext>();
                    var _usermgr = serviceScope.ServiceProvider.GetService<UserManager<MetafarAccDbEntity>>();
                    var _rolmgr = serviceScope.ServiceProvider.GetService<RoleManager<RoleDbEntity>>();
                    context.Database.EnsureCreated();

                    var users = context.MetafarUsers.ToList();
                    if(users.Any())
                    {
                        return;
                    }

                    var from = 100000.00m;
                    var to = 800000.00m;
                    var startAccountValance = RandomNumberGenerator.GenerateRandomDecimal(from, to);

                    var user = new MetafarAccDbEntity()
                    {
                        UserName = CryptographyHelper.ComputeHashSha256("371449635398431"),
                        AccountNumber = "999-999",
                        PasswordHash = CryptographyHelper.ComputeHashSha256("456"),
                        Name = "Mariano Pagani",
                        AccountBalance = startAccountValance,
                        Movements = new List<AccountMovementDbEntity>()
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    var movements = new List<AccountMovementDbEntity>();
                    for (int i = 0; i < 20; i++)
                    {
                        var type = RandomNumberGenerator.GenerateRandomNumber(0, 1);
                        var value = RandomNumberGenerator.GenerateRandomDecimal(0, user.AccountBalance /10 );

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

                    user.LastExtraction = movements.Where(x => x.Type == 0).OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;

                    context.AccountMovements.AddRange(movements);
                    context.SaveChanges();
                }
                catch (System.Exception ex)
                {
                    throw;
                }

            }
        }
    }
}
