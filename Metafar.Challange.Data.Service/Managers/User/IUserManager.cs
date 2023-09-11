namespace Metafar.Challange.Data.Service.Managers.User
{
    using Metafar.Challange.Data.Service.Managers.Models;
        
    public interface IUserManager
    {
        Task<Response<User>> FindUserAsync(string creditCard, string pin);

        Task SigInAsync(Guid id, string refreshToken = null, DateTime? expiration = null);

        Task<Response<User>> GetBalanceAsync(Guid id, string creditCard);

        Task<Response<WithdrawalResponse>> Withdrawal(Guid id, string creditCard, decimal amount);

        Task<Response<HistoryResponse>> History(Guid id, string creditCard, int start, int length, DateTime? from, DateTime? to);
    }
}
