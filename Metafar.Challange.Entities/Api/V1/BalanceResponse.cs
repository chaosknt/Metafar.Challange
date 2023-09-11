namespace Metafar.Challange.Entities.Api.V1
{
    public class BalanceResponse : BasicResponse
    {
        public BalanceResponse(string status, Guid correlation, string name, string accountNumber, decimal balance, DateTime? lastExtract) 
            : base(status, correlation)
        {
            this.User = new UserResponse()
            {
                Name = name,
                AccountNumber = accountNumber,
                Balance = balance,
                LastExtract = lastExtract
            };
        }

        public UserResponse User { get; set; }
    }
}
