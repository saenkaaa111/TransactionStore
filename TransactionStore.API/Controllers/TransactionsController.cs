﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using Marvelous.Contracts;
using TransactionStore.API.Models;

namespace TransactionStore.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, IMapper mapper,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
        }

        // api/transaction/
        [HttpPost("deposit")]
        [SwaggerOperation(Summary = "Add deposit")]
        [SwaggerResponse(StatusCodes.Status201Created, "Deposit added", typeof(int))]
        public ActionResult<int> AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Запрос на добавление Deposit в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddDeposit(transactionModel);

            _logger.LogInformation($"Транзакция типа Deposit с id = {transactionId} успешно добавлена");

            return StatusCode(201, transactionId);
        }

        // api/transaction/
        [HttpPost("transfer")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transfer successful", typeof(List<int>))]
        public ActionResult<List<int>> AddTransfer([FromBody] TransferRequestModel transfer)
        {
            _logger.LogInformation("Запрос на добавление Transfer в контроллере");

            var transferModel = _mapper.Map<TransferModel>(transfer);
            var transferIds = _transactionService.AddTransfer(transferModel);
            
            _logger.LogInformation($"Транзакция типа Transfer с id = {transferIds.First()}, {transferIds.Last()} успешно добавлены");

            return StatusCode(StatusCodes.Status200OK, transferIds);
        }

        [HttpPost("withdraw")]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(StatusCodes.Status200OK, "Withdraw successful", typeof(int))]
        public ActionResult<int> Withdraw([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Запрос на добавление Withdraw в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Транзакция типа Withdraw с id = {transactionId} успешно добавлена");

            return StatusCode(201, transactionId);
        }
        
        // api/transaction/
        [HttpGet("transaction/{accountId}")]
        [SwaggerOperation(Summary = "Get transactions by accountId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<TransactionResponseModel>))]
        public ActionResult<List<TransactionResponseModel>> GetTransactionsByAccountId(int accountId)
        {
            _logger.LogInformation($"Запрос на получение всех транзакций по AccountId = {accountId} в контроллере");

            var transactionModel = _transactionService.GetTransactionsByAccountId(accountId);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModel);

            _logger.LogInformation($"Транзакция по AccountId = {accountId} успешно получены");

            return Ok(transactions);
        }

        // api/Transactions/by-accountIds?accountIds=1&accountIds=2
        [HttpGet("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<TransactionResponseModel>))]
        public ActionResult<List<TransactionResponseModel>> GetTransactionsByAccountIds([FromQuery] List<int> accountIds)
        {
            _logger.LogInformation($"Запрос на получение всех транзакций по AccountIds  в контроллере");

            var transactionModels = _transactionService.GetTransactionsByAccountIds(accountIds);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModels);

            _logger.LogInformation($"Транзакция по AccountIds = {accountIds} успешно получены");

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get transaction by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(TransactionResponseModel))]
        public ActionResult<TransactionResponseModel> GetTransactionById(int id)
        {
            _logger.LogInformation($"Запрос на получение транзакции по Id = {id} в контроллере");

            var transactionModel = _transactionService.GetTransactionById(id);
            var transaction = _mapper.Map<TransactionResponseModel>(transactionModel);
            
            _logger.LogInformation($"Транзакция по Id = {id} успешно получена");

            return Ok(transaction);
        }
        
        [HttpGet("balanse-by-{accountId}")]
        [SwaggerOperation(Summary = "Get balanse by accountId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(decimal))]
        public ActionResult<decimal> GetBalanceByAccountId(int accountId)
        {
            _logger.LogInformation($"Запрос на получение баланса по accountId = {accountId} в контроллере");

            var balance = _transactionService.GetBalanceByAccountId(accountId);
            
            _logger.LogInformation($"Баланс успешно получен");

            return Ok(balance);
        }
    }
}
