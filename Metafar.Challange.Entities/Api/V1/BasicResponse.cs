namespace Metafar.Challange.Entities.Api.V1
{
    public class BasicResponse
    {
        public BasicResponse()
        {
        }

        public BasicResponse(string status, Guid correlation)
        {
            Status = status ?? throw new ArgumentNullException(nameof(status));
            CorrelationId = correlation;
        }

        public string Status { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
