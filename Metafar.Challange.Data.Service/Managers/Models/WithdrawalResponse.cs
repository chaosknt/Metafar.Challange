﻿namespace Metafar.Challange.Data.Service.Managers.Models
{
    public class WithdrawalResponse
    {
        public string AccountNumber { get; set; }

        public decimal? PreviousBalance { get; set; }

        public decimal? CurrentBalance { get; set; }

        public DateTime ExtratTime { get; set; }

        public string ErrorCode { get; set; }
    }
}
