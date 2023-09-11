using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Data;
using Metafar.Challange.Data.Models;
using Metafar.Challange.Data.Service.Managers.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metafar.Challange.Tests
{
    public static class DbContextMocked
    {
        public static Guid Userid = Guid.NewGuid();
        public static string Card = "11111111";
        public static string PIN = "123";

        public static List<AccountMovementDbEntity> Movements = new List<AccountMovementDbEntity>
                                        {
                                            new AccountMovementDbEntity
                                            {
                                                AccountMovementId = Guid.NewGuid(),
                                                Amount = 100000m,
                                                DateTime = DateTime.Now,
                                                UserId = Userid
                                            },
                                        };

        public static MetafarAccDbEntity User = new MetafarAccDbEntity()
        {
            Id = Userid,
            AccessFailedCount = 0,
            AccountBalance = 100000m,
            AccountNumber = "1234",
            CreditCardNumber = CryptographyHelper.ComputeHashSha256(Card),
            Pin = CryptographyHelper.ComputeHashSha256(PIN),
            LastExtraction = DateTime.UtcNow,
            Name = "name",
            LockoutEnabled = false
        };

        public static MetafarDbContext New(string db)
        {
            var options = new DbContextOptionsBuilder<MetafarDbContext>()
                            .UseInMemoryDatabase(databaseName: db)
                            .Options;

            var dbContext = new MetafarDbContext(options);

            dbContext.AccountMovements.AddRange(Movements);
            dbContext.MetafarUsers.Add(User);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
