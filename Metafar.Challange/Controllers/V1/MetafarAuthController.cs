using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Data.Service.Managers.User;
using Metafar.Challange.Entities.Api;
using Metafar.Challange.Entities.Api.V1;
using Metafar.Challange.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Illogger = Serilog;

namespace Metafar.Challange.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetafarAuthController : BaseApiController
    {
        private readonly IUserManager _userManager;
        private readonly JwtSettings jwtSettings;

        public MetafarAuthController(Illogger.ILogger logger, IUserManager userManager, JwtSettings jwtSettings) : base(logger)
        {
            this._userManager = userManager;
            this.jwtSettings = jwtSettings;
        }

        [HttpPost("sigin")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ErrorResponse>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BasicResponse>> Login([FromBody] LoginRequestModel request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));
            try
            {
                var result = await _userManager.FindUserAsync(request.CreditCardNumber, request.PIN);
                if (!result.WasSuccessfullyProcceded)
                {
                    return this.InvalidPayloadResponse(result.Message, correlationId);
                }

                var token = JwtHelpers.JwtHelpers.GenTokenkey(result.Content, jwtSettings);
                await this._userManager.SigInAsync(result.Content.Id, token.RefreshToken, token.ExpirationTime);

                return Ok(new AuthResponse(RequestStatus.Accepted.GetFriendlyName(), correlationId, token.Token, token.RefreshToken, token.ExpirationTime));
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the User Sig in, try again later", correlationId, loggerContext);
            }
        }

    }
}
