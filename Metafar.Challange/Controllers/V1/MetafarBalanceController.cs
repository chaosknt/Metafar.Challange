using Metafar.Challange.Common.Extensions;
using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Data.Service.Managers.User;
using Metafar.Challange.Data.Service.Stores.Movements;
using Metafar.Challange.Entities.Api;
using Metafar.Challange.Entities.Api.V1;
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

                return Ok(
                    new BalanceResponse(
                        RequestStatus.Accepted.GetFriendlyName(), 
                        correlationId,
                        result.Content.Name,
                        result.Content.AccountNumber,
                        result.Content.Balance, 
                        result.Content.LastExtract)
                    );
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the Balance method, try again later", correlationId, loggerContext);
            }
        }

        [HttpPost("retiro")]
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

                return Ok(
                     new WithdrawalResponse(
                         RequestStatus.Accepted.GetFriendlyName(),
                        correlationId,
                        result.Content.AccountNumber,
                        result.Content.CurrentBalance,
                        result.Content.PreviousBalance,
                        result.Content.ErrorCode,
                        result.Message,
                        result.Content.ExtratTime,
                        request.Amount)
                     );
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the Withdrawal method, try again later", correlationId, loggerContext);
            }
        }

        //                la api debe de contar con un endpoint el cual dado un número
        //de tarjeta debe retornar el historial de todas las operaciones realizadas. Dicha
        //respuesta debe de estar paginada, es decir, no debe devolver el historial todo junto,
        //sino que lo debe de hacer en páginas de 10 registros.

        [HttpGet("historial")]
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

                return Ok(new HistoryResponse()
                {
                    Status = RequestStatus.Accepted.GetFriendlyName(),
                    CorrelationId = correlationId,
                    User = new UserResponse()
                    {
                        AccountNumber = result.Content.User.AccountNumber,
                        Balance = result.Content.User.Balance,
                        LastExtract = result.Content.User.LastExtract,
                        Name = result.Content.User.Name,
                    },
                    ItemParcial = result.Content.ItemParcial,
                    ItemTotal = result.Content.ItemTotal,
                    Items = result.Content.Items.Select(x => new Movement()
                    {
                        Amount = x.Amount,
                        DateTime = x.DateTime,
                        Type = x.Type
                    })
                });
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the history, try again later", correlationId, loggerContext);
            }
        }
    }
}