using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services.Interfaces;
using Marvelous.Contracts;
using TransactionStore.API.Models;
using NLog;

namespace TransactionStore.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly Logger _logger;

        public TransactionsController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // api/transaction/
        [HttpPost("deposit")]
        [SwaggerOperation(Summary = "Add deposit")]
        [SwaggerResponse(201, "Deposit added")]
        public ActionResult AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            _logger.Debug("Запрос на добавление Deposit в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddDeposit(transactionModel);

            _logger.Debug($"Транзакция типа Deposit с id = {transactionId} успешно добавлена");

            return StatusCode(201, transactionId);
        }

        // api/transaction/
        [HttpPost("transfer")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(201, "Transfer successful")]
        public ActionResult AddTransfer([FromBody] TransferRequestModel transaction)
        {
            _logger.Debug("Запрос на добавление Transfer в контроллере");

            var transactionModel = _mapper.Map<TransferModel>(transaction);
            var transactionIds = _transactionService.AddTransfer(transactionModel);
            
            _logger.Debug($"Транзакция типа Transfer с id = {transactionIds.First()}, {transactionIds.Last()} успешно добавлены");

            return StatusCode(201, transactionIds);
        }

        [HttpPost("withdraw")]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(201, "Withdraw successful")]
        public ActionResult Withdraw([FromBody] TransactionRequestModel transaction)
        {
            _logger.Debug("Запрос на добавление Withdraw в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.Withdraw(transactionModel);

            _logger.Debug($"Транзакция типа Withdraw с id = {transactionId} успешно добавлена");

            return StatusCode(201, transactionId);
        }
        
        // api/transaction/
        [HttpGet("transaction/{accountId}")]
        [SwaggerOperation(Summary = "Get transactions by accountId")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetByAccountId(int accountId)
        {
            _logger.Debug($"Запрос на получение всех транзакций по AccountId = {accountId} в контроллере");

            var transactionModel = _transactionService.GetByAccountId(accountId);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModel);

            _logger.Debug($"Транзакция по AccountId = {accountId} успешно получены");

            return Ok(transactions);
        }

        // api/Transactions/by-accountIds?accountIds=1&accountIds=2
        [HttpGet("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetTransactionsByAccountIds([FromQuery] List<int> accountIds)
        {
            _logger.Debug($"Запрос на получение всех транзакций по AccountIds  в контроллере");

            var transactionModels = _transactionService.GetTransactionsByAccountIds(accountIds);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModels);

            _logger.Debug($"Транзакция по AccountIds = {accountIds} успешно получены");

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get transaction by id")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetTransactionById(int id)
        {
            _logger.Debug($"Запрос на получение транзакции по Id = {id} в контроллере");

            var transactionModel = _transactionService.GetTransactionById(id);
            var transaction = _mapper.Map<TransactionResponseModel>(transactionModel);
            
            _logger.Debug($"Транзакция по Id = {id} успешно получена");

            return Ok(transaction);
        }
        
        
        [HttpGet("balanse-by-{accountId}")]
        [SwaggerOperation(Summary = "Get balance by accountId")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetBalanceByAccountId(int accountId)
        {
            _logger.Debug($"Запрос на получение баланса по accountId = {accountId} в контроллере");

            var balance = _transactionService.GetBalanceByAccountId(accountId);
            
            _logger.Debug($"Баланс успешно получен");

            return Ok(balance);
        }
        
        
        [HttpGet("balanse-by-accountIds")]
        [SwaggerOperation(Summary = "Get balance by accountIds")]
        [SwaggerResponse(200, "OK")]
        public ActionResult GetBalanceByAccountId([FromQuery] List<int> accountIds)
        {
            
            var balance = _transactionService.GetBalanceByAccountIds(accountIds);
            
            _logger.Debug($"Баланс успешно получен");

            return Ok(balance);
        }
    }
}
