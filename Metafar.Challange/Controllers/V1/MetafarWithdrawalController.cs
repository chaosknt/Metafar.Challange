using Metafar.Challange.Data.Service.Managers.Models;
using Metafar.Challange.Entities.Api.V1;
using Metafar.Challange.Entities.Api;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Metafar.Challange.Data.Service.Managers.User;
using Illogger = Serilog;
using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Mappers;

namespace Metafar.Challange.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MetafarWithdrawalController : BaseApiController
    {
        private readonly IUserManager _userManager;

        public MetafarWithdrawalController(Illogger.ILogger logger, IUserManager userManager) : base(logger)
        {
            this._userManager = userManager;
        }

        [HttpPatch("retiro")]
        [ProducesResponseType(typeof(Entities.Api.V1.WithdrawalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BasicResponse>> Withdrawal([FromBody] RetiroRequestModel request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
                var result = await _userManager.Withdrawal(User.GetUserId(), request.CreditCardNumber, request.Amount);
                if (!result.WasSuccessfullyProcceded)
                {
                    return this.InvalidPayloadResponse(result.Message, correlationId);
                }

                return Ok(result.ToResponse(RequestStatus.Accepted, correlationId, request.Amount));
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the Withdrawal method, try again later", correlationId, loggerContext);
            }
        }
    }
}
