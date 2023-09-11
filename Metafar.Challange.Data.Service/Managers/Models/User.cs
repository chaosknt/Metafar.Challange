namespace Metafar.Challange.Data.Service.Managers.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public DateTime? LastExtract { get; set; }

        public string LastExtractForMated => this.LastExtract.HasValue ? this.LastExtract.Value.ToString("dd-MM-yyyy HH:mm:ss") : string.Empty;
    }
}
