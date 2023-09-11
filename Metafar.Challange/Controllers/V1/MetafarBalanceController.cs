using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Data.Service.Managers.User;
using Metafar.Challange.Entities.Api;
using Metafar.Challange.Entities.Api.V1;
using Metafar.Challange.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Illogger = Serilog;

namespace Metafar.Challange.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MetafarBalanceController : BaseApiController
    {
        private readonly IUserManager _userManager;

        public MetafarBalanceController(Illogger.ILogger logger, IUserManager userManager) : base(logger)
        {
            this._userManager = userManager;
        }

        [HttpPost("balance")]
        [ProducesResponseType(typeof(BalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BasicResponse>> Balance([FromBody] BaseRequestModel request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
                var result = await _userManager.GetBalanceAsync(User.GetUserId(), request.CreditCardNumber);
                if (!result.WasSuccessfullyProcceded)
                {
                    return this.InvalidPayloadResponse(result.Message, correlationId);
                }

                return Ok(result.ToResponse(RequestStatus.Accepted, correlationId));
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the Balance method, try again later", correlationId, loggerContext);
            }
        }
    }
}