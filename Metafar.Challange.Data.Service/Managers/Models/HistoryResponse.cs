namespace Metafar.Challange.Data.Service.Managers.Models
{
    public class HistoryResponse
    {
        public User User { get; set; }

        public IEnumerable<Movement> Items { get; set; }

        public int ItemTotal { get; set; }

        public int ItemParcial { get; set; }
    }
}
