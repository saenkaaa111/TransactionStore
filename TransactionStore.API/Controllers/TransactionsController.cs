﻿using AutoMapper;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
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
            _logger.LogInformation("Request to add Deposit in the controller");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = await _transactionService.AddDeposit(transactionModel);

            _logger.LogInformation($"Deposit with id = {transactionId} added");
            await _transactionProducer.Main(transactionId);

            return Ok(transactionId);
        }

        // api/transaction/
        [HttpPost("transfer")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transfer successful", typeof(List<long>))]
        public async Task<ActionResult<List<long>>> AddTransfer([FromBody] TransferRequestModel transfer)
        {
            _logger.LogInformation("Request to add Transfer in the controller");

            var transferModel = _mapper.Map<TransferModel>(transfer);
            var transferIds = await _transactionService.AddTransfer(transferModel);

            _logger.LogInformation($"Transfer added");
            await _transactionProducer.Main(transferIds[0]);
            await _transactionProducer.Main(transferIds[1]);

            return Ok(transferIds);
        }

        [HttpPost("withdraw")]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(StatusCodes.Status200OK, "Withdraw successful", typeof(long))]
        public async Task<ActionResult<long>> Withdraw([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Request to add Withdraw in the controller");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            transactionModel.Type = TransactionType.Withdraw;
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Withdraw with id = {transactionId} added");
            await _transactionProducer.Main(transactionId);

            return Ok(transactionId);
        }

        // api/transaction/
        [HttpGet("transaction/{accountId}")]
        [SwaggerOperation(Summary = "Get transactions by accountId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(ArrayList))]
        public async Task<ActionResult<ArrayList>> GetTransactionsByAccountId(int accountId)
        {
            _logger.LogInformation($"Request to receive all transactions by AccountId = {accountId} in the controller");

            var transactionModel = await _transactionService.GetTransactionsByAccountId(accountId);
            var transactions = _mapper.Map<ArrayList>(transactionModel);

            _logger.LogInformation($"Transactions by AccountId = {accountId} received");

            return Ok(transactions);
        }

        // api/Transactions/by-accountIds?accountIds=1&accountIds=2
        [HttpGet("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<TransactionResponseModel>))]
        public async Task<ActionResult<List<TransactionResponseModel>>> GetTransactionsByAccountIds([FromQuery] List<int> accountIds)
        {
            _logger.LogInformation($"Request to receive all transactions by AccountIds in the controller");

            var transactionModels = await _transactionService.GetTransactionsByAccountIds(accountIds);
            var transactions = _mapper.Map<List<TransactionResponseModel>>(transactionModels);

            _logger.LogInformation($"Transactions by AccountIds received");

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get transaction by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(TransactionResponseModel))]
        public async Task<ActionResult<TransactionResponseModel>> GetTransactionById(long id)
        {
            _logger.LogInformation($"Request to receive transaction by Id = {id} in the controller");

            var transactionModel = await _transactionService.GetTransactionById(id);
            var transaction = _mapper.Map<TransactionResponseModel>(transactionModel);

            _logger.LogInformation($"Transactions by AccountId = {id} received");

            return Ok(transaction);
        }

        [HttpPost("service-payment")]
        [SwaggerOperation(Summary = "Service payment")]
        [SwaggerResponse(StatusCodes.Status200OK, "Payment successful", typeof(long))]
        public async Task<ActionResult<long>> ServicePayment([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Request to add Service payment in the conroller");

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            transactionModel.Type = TransactionType.ServicePayment;
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Service payment with Id = {transactionId} added");

            return Ok(transactionId);
        }
    }
}
