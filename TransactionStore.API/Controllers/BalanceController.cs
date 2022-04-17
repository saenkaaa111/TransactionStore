using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Controllers
{
    [ApiController]
    [Route(TransactionEndpoints.ApiBalance)]
    public class BalanceController : Controller
    {
        private readonly IBalanceService _balanceService;
        private readonly ILogger<BalanceController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRequestHelper _requestHelper;

        public BalanceController(IBalanceService balanceService, ILogger<BalanceController> logger,
            IRequestHelper requestHelper, IConfiguration configuration) 
        {
            _balanceService = balanceService;
            _logger = logger;
            _configuration = configuration;
            _requestHelper = requestHelper;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get balance by accountIds in given currency")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(decimal))]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<int>>> GetBalanceByAccountIdsInGivenCurrency(
            [FromQuery] List<int> id, [FromQuery] Currency currency)
        {
            _logger.LogInformation("Request to receive a balance by AccountIds in the controller");

            await CheckMicroservice(Microservice.MarvelousCrm);

            var balance = await _balanceService.GetBalanceByAccountIdsInGivenCurrency(id, currency);

            _logger.LogInformation("Balance received");

            return Ok(balance);
        }

        private async Task CheckMicroservice(params Microservice[] service)
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var identity = await _requestHelper
                .SendRequestCheckValidateToken(_configuration[Microservice.MarvelousAuth.ToString()],
                AuthEndpoints.ApiAuth + AuthEndpoints.ValidationMicroservice, token);

            if (!service.Select(r => r.ToString()).Contains(identity.IssuerMicroservice))
            {
                throw new ForbiddenException($"{identity.IssuerMicroservice} doesn't have access to this endpiont");
            }
        }
    }
}
