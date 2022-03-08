using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransactionStore.API.Models;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services.Interfaces;

namespace TransactionStore.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // api/transaction/
        [HttpPost("deposit")]
        [SwaggerOperation(Summary = "Add deposit")]
        [SwaggerResponse(201, "Deposit added")]
        public ActionResult AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddDeposit(transactionModel);

            return StatusCode(201, transactionId);
        }

        // api/transaction/
        [HttpPost("transfer-to-{accountId}-in-{currencyTo}")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(201, "List transactions by accountId ")]
        public ActionResult AddTransfer([FromBody] TransactionRequestModel transaction, int accountId, int currencyTo)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddTransfer(transactionModel, accountId, currencyTo);

            return StatusCode(201, transactionId);
        }

        [HttpPost("withdraw")]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(201, "Withdraw successful")]
        public ActionResult Withdraw([FromBody] TransactionRequestModel transaction)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.Withdraw(transactionModel);

            return StatusCode(201, transactionId);
        }
        
        // api/transaction/
        [HttpGet("{accountId}")]
        [SwaggerOperation(Summary = "Get transactions by accountId")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetByAccountId(int accountId)
        {
            var transactionModel = _transactionService.GetByAccountId(accountId);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModel);

            return Ok(transactions);
        }

        [HttpPost("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetByAccountIds(List<int> accountIds)
        {
            var transactionModels = _transactionService.GetByAccountIds(accountIds);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModels);

            return Ok(transactions);
        }
    }
}
