﻿using AutoMapper;
using Marvelous.Contracts.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using TransactionStore.API.Models;
using TransactionStore.API.Producers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;
        private readonly ITransactionProducer _transactionProducer;

        public TransactionsController(ITransactionService transactionService, IMapper mapper,
            ILogger<TransactionsController> logger, ITransactionProducer transactionProducer)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
            _transactionProducer = transactionProducer;
        }

        // api/transaction/
        [HttpPost("deposit")]
        [SwaggerOperation(Summary = "Add deposit")]
        [SwaggerResponse(StatusCodes.Status201Created, "Deposit added", typeof(long))]
        public async Task<ActionResult<long>> AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Запрос на добавление Deposit в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = await _transactionService.AddDeposit(transactionModel);

            _logger.LogInformation($"Транзакция типа Deposit с id = {transactionId} успешно добавлена");
            await _transactionProducer.Main(transactionId);
            return Ok(transactionId);
        }

        // api/transaction/
        [HttpPost("transfer")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transfer successful", typeof(List<long>))]
        public async Task<ActionResult<List<long>>> AddTransfer([FromBody] TransferRequestModel transfer)
        {
            _logger.LogInformation("Запрос на добавление Transfer в контроллере");

            var transferModel = _mapper.Map<TransferModel>(transfer);

            var transferIds = await _transactionService.AddTransfer(transferModel);

            _logger.LogInformation($"Транзакция типа Transfer успешно добавлены");
            await _transactionProducer.Main(transferIds[0]);
            await _transactionProducer.Main(transferIds[1]);
            return Ok(transferIds);
        }

        [HttpPost("withdraw")]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(StatusCodes.Status200OK, "Withdraw successful", typeof(long))]
        public async Task<ActionResult<long>> Withdraw([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Запрос на добавление Withdraw в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Транзакция типа Withdraw с id = {transactionId} успешно добавлена");
            await _transactionProducer.Main(transactionId);
            return Ok(transactionId);
        }

        // api/transaction/
        [HttpGet("transaction/{accountId}")]
        [SwaggerOperation(Summary = "Get transactions by accountId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ArrayList))]
        public async Task<ActionResult<ArrayList>> GetTransactionsByAccountId(long accountId)
        {
            _logger.LogInformation($"Запрос на получение всех транзакций по AccountId = {accountId} в контроллере");

            var transactionModel = await _transactionService.GetTransactionsByAccountId(accountId);
            var transactions = _mapper.Map<ArrayList>(transactionModel);

            _logger.LogInformation($"Транзакция по AccountId = {accountId} успешно получены");

            return Ok(transactions);
        }

        // api/Transactions/by-accountIds?accountIds=1&accountIds=2
        [HttpGet("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<TransactionResponseModel>))]
        public async Task<ActionResult<List<TransactionResponseModel>>> GetTransactionsByAccountIds([FromQuery] List<long> accountIds)
        {
            _logger.LogInformation($"Запрос на получение всех транзакций по AccountIds  в контроллере");

            var transactionModels = await _transactionService.GetTransactionsByAccountIds(accountIds);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModels);

            _logger.LogInformation($"Транзакция по AccountIds = {accountIds} успешно получены");

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get transaction by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(TransactionResponseModel))]
        public async Task<ActionResult<TransactionResponseModel>> GetTransactionById(long id)
        {
            _logger.LogInformation($"Запрос на получение транзакции по Id = {id} в контроллере");

            var transactionModel = await _transactionService.GetTransactionById(id);
            var transaction = _mapper.Map<TransactionResponseModel>(transactionModel);

            _logger.LogInformation($"Транзакция по Id = {id} успешно получена");

            return Ok(transaction);
        }

        [HttpGet("balanse-by-{accountId}")]
        [SwaggerOperation(Summary = "Get balanse by accountId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(decimal))]
        public async Task<ActionResult<decimal>> GetBalanceByAccountId(long accountId)
        {
            _logger.LogInformation($"Запрос на получение баланса по accountId = {accountId} в контроллере");

            var balance = await _transactionService.GetBalanceByAccountId(accountId);

            _logger.LogInformation($"Баланс успешно получен");

            return Ok(balance);
        }


        [HttpGet("balanse-by-accountIds")]
        [SwaggerOperation(Summary = "Get balance by accountIds")]
        [SwaggerResponse(200, "OK")]
        public async Task<ActionResult> GetBalanceByAccountId([FromQuery] List<long> accountIds)
        {

            var balance = await _transactionService.GetBalanceByAccountIds(accountIds);

            _logger.LogInformation($"Баланс успешно получен");

            return Ok(balance);
        }

        [HttpPost("service-payment")]
        [SwaggerOperation(Summary = "Service payment")]
        [SwaggerResponse(StatusCodes.Status200OK, "Payment successful", typeof(int))]
        public async Task<ActionResult<long>> ServicePayment([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Запрос на добавление Service payment в контроллере");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Транзакция типа Service payment с id = {transactionId} успешно добавлена");

            return Ok(transactionId);
        }
    }
}
