using Metafar.Challange.Entities.Api.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using Illogger = Serilog;

namespace Metafar.Challange.Controllers.V1
{
    public class MetafarBalanceController : BaseApiController
    {
        public MetafarBalanceController(Illogger.ILogger logger) : base(logger)
        {
        }

        public async Task<ActionResult<BasicResponse>> Balance([FromBody] string request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
                //la api debe de contar con un endpoint el cual dado un nro de tarjeta
                //retorne la siguiente informacion: nombre del usuario, numero de cuenta, saldo actual y
                //fecha de la última extracción.
                return Ok(new { });
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the User Sig in, try again later", correlationId, loggerContext);
            }
        }

        [HttpGet]
        public async Task<ActionResult<BasicResponse>> Retiro([FromBody] string request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {

//                la api debe contar con un endpoint el cual dado un número de tarjeta
//y un montón, le permita realizar una extracción.En caso de que el monto a retirar sea
//superior al saldo disponible de la tarjeta, el endpoint debe de retornar un código de
//error.En caso de que todo sea correcto se debe retornar un resumen de la operación
//realizada.

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the User Sig in, try again later", correlationId, loggerContext);
            }
        }

        [HttpGet]
        public async Task<ActionResult<BasicResponse>> Historial([FromBody] string request)
        {
            var correlationId = Guid.NewGuid();
            var loggerContext = this.logger
                .ForContext("CorrelationId", correlationId)
                .ForContext("RequestBody", JsonConvert.SerializeObject(request));

            try
            {
//                la api debe de contar con un endpoint el cual dado un número
//de tarjeta debe retornar el historial de todas las operaciones realizadas. Dicha
//respuesta debe de estar paginada, es decir, no debe devolver el historial todo junto,
//sino que lo debe de hacer en páginas de 10 registros.

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return base.ErrorResponse(ex, "There was an error processing the User Sig in, try again later", correlationId, loggerContext);
            }
        }
    }
}

//https://dev.to/ebarrioscode/patron-repositorio-repository-pattern-y-unidad-de-trabajo-unit-of-work-en-asp-net-core-webapi-3-0-5goj