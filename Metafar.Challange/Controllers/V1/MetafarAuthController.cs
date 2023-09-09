using Metafar.Challange.Entities.Api.V1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Illogger = Serilog;

namespace Metafar.Challange.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    public class MetafarAuthController : BaseApiController
    {
        public MetafarAuthController(Illogger.ILogger logger) : base(logger)
        {
        }

        [HttpPost]
        public async Task<ActionResult<BasicResponse>> Login([FromBody] string request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
                //loggerContext.Information("Start User Sig in");
                //var result = await this.mapaSigInService.CheckPasswordAsync(request.UserName, request.Password);
                //var token = JwtHelpers.GenTokenkey(result, jwtSettings);
                //await this.mapaSigInService.UserSigInAsync(Guid.Parse(result.UserId), token.RefreshToken, token.ExpirationTime);
                return Ok(new { });
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the User Sig in, try again later", correlationId, loggerContext);
            }
        }

    }
}
