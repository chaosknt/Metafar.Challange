using System.Text.Json.Serialization;

namespace Metafar.Challange.Entities.Api.V1
{
    public class WithdrawalResponse : BasicResponse
    {
        public WithdrawalResponse(
            string statusCode, 
            Guid correlationId,
            string accountNumber, 
            decimal? currentBalance,
            decimal? previosBalance,
            string errorCode, 
            string errorMessage, 
            DateTime? extractTime,
            decimal requestedAmount) : base(statusCode, correlationId)
        {
            this.AccountNumber = accountNumber;
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.CurrentBalance = currentBalance;
            this.PreviousBalance = previosBalance;
            this.ExtratTime = extractTime;
            this.RequestedAmount = requestedAmount;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? AccountNumber { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? PreviousBalance { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? CurrentBalance { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? ExtratTime { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        public decimal RequestedAmount { get; set; }
    }
}
