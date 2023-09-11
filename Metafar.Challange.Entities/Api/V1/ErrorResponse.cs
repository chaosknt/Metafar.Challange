namespace Metafar.Challange.Entities.Api.V1
{
    public class ErrorResponse : BasicResponse
    {
        public ErrorResponse(string status, Guid correlationId, string errorMessa, int errCode)
            :base(status, correlationId)
        {
            this.ErrorMessage = errorMessa;
            this.ErrorCode = errCode;
        }

        public ErrorResponse()
        {
        }

        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }
    }
}
