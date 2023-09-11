using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Data.Service.Managers;
using Metafar.Challange.Data.Service.Managers.Models;
using Metafar.Challange.Entities.Api;
using Metafar.Challange.Entities.Api.V1;

namespace Metafar.Challange.Mappers
{
    public static class ResponseExtensions
    {
        public static BalanceResponse ToResponse(this Response<User> result, RequestStatus status, Guid correlationId)
            => result == null  && result.Content != null?
                default 
                : new BalanceResponse(
                    status.GetFriendlyName(),
                    correlationId, 
                    result.Content.Name,
                    result.Content.AccountNumber,
                    result.Content.Balance,
                    result.Content.LastExtract);


        public static Metafar.Challange.Entities.Api.V1.WithdrawalResponse ToResponse(
            this Response<Data.Service.Managers.Models.WithdrawalResponse> result, 
            RequestStatus status, 
            Guid correlationId, 
            decimal amount)
         => result == null && result.Content != null ?
                default
                : new Metafar.Challange.Entities.Api.V1.WithdrawalResponse(
                    status.GetFriendlyName(),
                    correlationId,
                    result.Content.AccountNumber,
                        result.Content.CurrentBalance,
                        result.Content.PreviousBalance,
                        result.Content.ErrorCode,
                        result.Message,
                        result.Content.ExtratTime,
                       amount);


        public static Entities.Api.V1.HistoryResponse ToResponse(
            this Response<Data.Service.Managers.Models.HistoryResponse> result, 
            RequestStatus status,
            Guid correlationId)
        {
            return new Entities.Api.V1.HistoryResponse()
            {
                Status = RequestStatus.Accepted.GetFriendlyName(),
                CorrelationId = correlationId,
                User = new UserResponse()
                {
                    AccountNumber = result.Content.User.AccountNumber,
                    Balance = result.Content.User.Balance,
                    LastExtract = result.Content.User.LastExtract,
                    Name = result.Content.User.Name,
                },
                ItemParcial = result.Content.ItemParcial,
                ItemTotal = result.Content.ItemTotal,
                Items = result.Content.Items.Select(x => new Entities.Api.V1.Movement()
                {
                    Amount = x.Amount,
                    DateTime = x.DateTime,
                    Type = x.Type
                })
            };
        }
    }
}
