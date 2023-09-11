namespace Metafar.Challange.Entities.Api.V1
{
    public class UserResponse
    {
        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public DateTime? LastExtract { get; set; }

        public string LastExtractForMated => this.LastExtract.HasValue ? this.LastExtract.Value.ToString("dd-MM-yyyy HH:mm:ss") : string.Empty;
    }
}
