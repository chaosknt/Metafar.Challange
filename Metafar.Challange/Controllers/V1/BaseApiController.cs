using Metafar.Challange.Common.Enum;
using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Entities.Api;
using Metafar.Challange.Entities.Api.V1;
using Microsoft.AspNetCore.Mvc;
using Illogger = Serilog;


namespace Metafar.Challange.Controllers.V1
{
    public class BaseApiController : ControllerBase
    {
        internal readonly Illogger.ILogger logger;

        public BaseApiController(Illogger.ILogger logger)
        {
            this.logger = logger;
        }

        internal ActionResult<BasicResponse> InvalidPayloadResponse(string errorMessage, Guid correlationId)
        {
            return BadRequest(new ErrorResponse { Status = RequestStatus.Error.GetFriendlyName(), ErrorMessage = errorMessage, ErrorCode = KnownErrorCodesEnum.InvalidPayload.AsInt(), CorrelationId = correlationId });
        }

        internal ActionResult<BasicResponse> UnauthorizedResponse(Guid correlationId)
        {
            return Unauthorized(new BasicResponse { Status = RequestStatus.Unauthorized.GetFriendlyName(), CorrelationId = correlationId });
        }

        internal ActionResult<BasicResponse> ErrorResponse(Exception exception, Guid correlationId, Serilog.ILogger loggerContext)
            => ErrorResponse(exception, exception.Message, correlationId, loggerContext);

        internal ActionResult<BasicResponse> ErrorResponse(Exception exception, string errorMessage, Guid correlationId, Serilog.ILogger loggerContext)
        {
            loggerContext = loggerContext.ForContext("StackTrace", exception.StackTrace);
            loggerContext = loggerContext.ForContext("ErrorMessage", exception.Message);
            if (exception.InnerException != null)
            {
                loggerContext = loggerContext.ForContext("InnerException", exception.InnerException.Message);
            }

            loggerContext.Error(exception, errorMessage);

            return BadRequest(new ErrorResponse { Status = RequestStatus.Error.GetFriendlyName(), ErrorMessage = errorMessage, ErrorCode = KnownErrorCodesEnum.UnknownError.AsInt(), CorrelationId = correlationId });
        }

        internal ActionResult<BasicResponse> FailedResponse(Exception exception, KnownErrorCodesEnum errorCode, Guid correlationId, Serilog.ILogger loggerContext)
            => FailedResponse(exception, exception.Message, errorCode, correlationId, loggerContext);

        internal ActionResult<BasicResponse> FailedResponse(Exception exception, string errorMessage, KnownErrorCodesEnum errorCode, Guid correlationId, Illogger.ILogger loggerContext)
        {
            loggerContext.Error(exception, errorMessage);

            return BadRequest(new ErrorResponse { Status = RequestStatus.Failed.GetFriendlyName(), ErrorMessage = errorMessage, ErrorCode = errorCode.AsInt(), CorrelationId = correlationId });
        }
    }
}
