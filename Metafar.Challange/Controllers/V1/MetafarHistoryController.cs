using Metafar.Challange.Data.Service.Managers.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Illogger = Serilog;
using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Mappers;
using Metafar.Challange.Entities.Api.V1;
using Metafar.Challange.Entities.Api;
using Newtonsoft.Json;

namespace Metafar.Challange.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MetafarHistoryController : BaseApiController
    {
        private readonly IUserManager _userManager;

        public MetafarHistoryController(Illogger.ILogger logger, IUserManager userManager) : base(logger)
        {
            this._userManager = userManager;
        }

        [HttpGet("historial")]
        [ProducesResponseType(typeof(HistoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BasicResponse>> History([FromQuery] HistoryRequestModel request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
                if (!request.IsValidModel())
                {
                    return this.InvalidPayloadResponse(request.ValidationMessage(), correlationId);
                }

                var result = await _userManager.History(
                                            User.GetUserId(),
                                            request.CreditCardNumber,
                                            request.Start ?? 0,
                                            request.Length ?? 10,
                                            request.From(),
                                            request.To());

                if (!result.WasSuccessfullyProcceded)
                {
                    return this.InvalidPayloadResponse(result.Message, correlationId);
                }

                return Ok(result.ToResponse(RequestStatus.Accepted, correlationId));
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the history, try again later", correlationId, loggerContext);
            }
        }
    }
}
