﻿namespace Metafar.Challange.Entities.Api.V1
{
    public class HistoryResponse : BasicResponse
    {
        public UserResponse User { get; set; }

        public IEnumerable<Movement> Items { get; set; }

        public int ItemTotal { get; set; }

        public int ItemParcial { get; set; }
    }
}
