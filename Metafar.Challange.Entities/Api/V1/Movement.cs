namespace Metafar.Challange.Entities.Api.V1
{
    public class Movement
    {
        public decimal Amount { get; set; }

        public string Type { get; set; }

        public DateTime DateTime { get; set; }

        public string DateTimeParsed => this.DateTime.ToString("yyyy-MM-dd");
    }
}
