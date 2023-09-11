namespace Metafar.Challange.Data.Service.Managers.User
{
    using Metafar.Challange.Common.Helpers;
    using Metafar.Challange.Data.Models;
    using Metafar.Challange.Data.Service.Managers.Models;
    using Metafar.Challange.Data.Service.Mappers;
    using Metafar.Challange.Data.Service.Stores.Movements;
    using Metafar.Challange.Data.Service.Stores.User;
    using Metafar.Challange.Entities.Enum;
    using System;

    public class UserManager : IUserManager
    {
        private readonly IUserStore _userStore;
        private readonly IUserMovementsStore _userMovements;

        private const int _maxSigInRetries = 4;
        private const string _creditCardNotFound = "La tarjeta de credito ingresada no pertenece al usuario Logueado";

        public UserManager(IUserStore store, IUserMovementsStore userMovements)
        {
            this._userStore = store;
            this._userMovements = userMovements;
        }

        public async Task<Response<User>> FindUserAsync(string creditCard, string pin)
        {
            var encryptedPin = CryptographyHelper.ComputeHashSha256(pin);
            var user = await FindUserByCreditCardAsync(creditCard);

            if (user == null)
            {
                return new Response<User>() { WasSuccessfullyProcceded = false, Message = "El numero de la tarjeta de credito y/o el PIN son incorrectos"};
            }

            if(user.AccessFailedCount >= _maxSigInRetries)
            {
                return new Response<User>() { WasSuccessfullyProcceded = false, Message = "El usuario se encuentra bloqueado" };
            }

            if(user.Pin != encryptedPin)
            {
                user.AccessFailedCount++;
                if(user.AccessFailedCount >= _maxSigInRetries)
                {
                    user.LockoutEnabled = true;
                }

                await this._userStore.UpdateUserAsync(user);
                return new Response<User>() { WasSuccessfullyProcceded = false, Message = "El numero de la tarjeta de credito y/o el PIN son incorrectos" };
            }

            return new Response<User>() { WasSuccessfullyProcceded = true, Content = user.ToModel() };
        }

        public async Task SigInAsync(Guid id, string refreshToken = null, DateTime? expiration = null)
        {
            var user = (await this._userStore.GetAllAsync(x => x.Id == id)).FirstOrDefault();
            if(user == null)
            {
                return;
            }

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = expiration;
            await this._userStore.UpdateUserAsync(user);
        }

        public async Task<Response<User>> GetBalanceAsync(Guid id, string creditCard)
        {
            var user = await this.FindUserByCreditCardAsync(creditCard);

            if(user == null)
            {
                return new Response<User>() { WasSuccessfullyProcceded = false, Message = $"No se encontro ningun balance para la tarjeta de credito {creditCard}" };
            }

            if(user.Id != id)
            {
                return new Response<User>() { WasSuccessfullyProcceded = false, Message = _creditCardNotFound };
            }

            return new Response<User>() { WasSuccessfullyProcceded = true, Content = user.ToModel() };
        }

        public async Task<Response<WithdrawalResponse>> Withdrawal(Guid id, string creditCard, decimal amount)
        {
            var user = await this.FindUserByCreditCardAsync(creditCard);

            if (user.Id != id)
            {
                return new Response<WithdrawalResponse>() { WasSuccessfullyProcceded = false, Message = _creditCardNotFound };
            }

            var currentBalance = user.AccountBalance;
            if(amount > currentBalance)
            {
                return new Response<WithdrawalResponse>() 
                {
                    WasSuccessfullyProcceded = true, 
                    Message = "El saldo ingresado excede el dispoible",
                    Content = new WithdrawalResponse() { ErrorCode = ErrorCodes.InsufficientBalanceCode }
                };
            }
            
            var movement = await _userMovements.Withdrawal(user.Id, amount);
            return new Response<WithdrawalResponse>()
            {
                WasSuccessfullyProcceded = true,
                Content = new WithdrawalResponse()
                {
                    PreviousBalance = user.AccountBalance,
                    CurrentBalance = currentBalance,
                    AccountNumber = user.AccountNumber,
                    ExtratTime = movement.DateTime,
                }
            };
        }

        public async Task<Response<HistoryResponse>> History(Guid id, string creditCard, int start, int length, DateTime? from, DateTime? to)
        {
            var user = await this.FindUserByCreditCardAsync(creditCard);

            if (user.Id != id)
            {
                return new Response<HistoryResponse>() { WasSuccessfullyProcceded = false, Message = _creditCardNotFound };
            }

            var movements = await this._userMovements.GetAllAsync(x => x.UserId == user.Id);
            var partialData = movements.ToArray();

            if (from.HasValue && to.HasValue && from == to)
            {
                partialData = partialData.Where(x => x.DateTime.Date == from.Value.Date).ToArray();
            }
            else
            {
                if (from.HasValue)
                {
                    partialData = partialData.Where(x => x.DateTime >= from).ToArray();
                }

                if (to.HasValue)
                {
                    partialData = partialData.Where(y => y.DateTime <= to).ToArray();
                }
            }


            partialData = partialData.Skip(start).Take(length).ToArray();

            return new Response<HistoryResponse>()
            {
                WasSuccessfullyProcceded = true,
                Content = new HistoryResponse()
                {
                    User = user.ToModel(),
                    Items = partialData.Select(x => x.ToModel()),
                    ItemParcial = partialData.Length,
                    ItemTotal = movements.Count(),
                }
            };
        }

        private async Task<MetafarAccDbEntity> FindUserByCreditCardAsync(string creditCard)
           => (await this._userStore.GetAllAsync(x => x.CreditCardNumber == CryptographyHelper.ComputeHashSha256(creditCard))).FirstOrDefault();
        
    }
}
