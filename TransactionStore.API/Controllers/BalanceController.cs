using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.Urls;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Controllers
{
    [ApiController]
    [Route(TransactionUrls.ApiBalance)]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(IBalanceService balanceService, ILogger<BalanceController> logger)
        {
            _balanceService = balanceService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get balance by accountIds in given currency")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(decimal))]
        public async Task<ActionResult> GetBalanceByAccountIdsInGivenCurrency([FromQuery] BalanceRequestModel balanceRequestModel)
        {
            _logger.LogInformation($"Request to receive a balance by AccountIds in the controller");

            var balance = await _balanceService
                .GetBalanceByAccountIdsInGivenCurrency(balanceRequestModel.AccountIds, balanceRequestModel.Currency);

            _logger.LogInformation($"Balance received");

            return Ok(balance);
        }
    }
}
