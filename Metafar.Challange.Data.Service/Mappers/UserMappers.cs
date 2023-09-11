using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Data.Models;
using Metafar.Challange.Data.Service.Managers.Models;
using Metafar.Challange.Entities.Enum;

namespace Metafar.Challange.Data.Service.Mappers
{
    public static class UserMappers
    {
        public static User ToModel(this MetafarAccDbEntity metafarAccDbEntity)
            => metafarAccDbEntity == null ? default : new User()
            {
                Id = metafarAccDbEntity.Id,
                Name = metafarAccDbEntity.Name,
                AccountNumber = metafarAccDbEntity.AccountNumber,  
                Balance = metafarAccDbEntity.AccountBalance,
                LastExtract = metafarAccDbEntity.LastExtraction
            };

        public static Movement ToModel(this AccountMovementDbEntity accountMovementDbEntity)
        => accountMovementDbEntity == null ? default : new Movement()
            {
                Amount = accountMovementDbEntity.Amount,
                DateTime = accountMovementDbEntity.DateTime,
                Type = ((AccountMovementsEnum)accountMovementDbEntity.Type).GetFriendlyName()
            };
    }
}
