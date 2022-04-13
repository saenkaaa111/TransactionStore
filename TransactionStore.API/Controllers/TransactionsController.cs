using AutoMapper;
using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using TransactionStore.API.Extensions;
using TransactionStore.API.Producers;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Controllers
{
    [ApiController]
    [Route(TransactionEndpoints.ApiTransactions)]
    public class TransactionsController : AdvancedController
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;
        private readonly ITransactionProducer _transactionProducer;

        public TransactionsController(ITransactionService transactionService, IMapper mapper,
            ILogger<TransactionsController> logger, ITransactionProducer transactionProducer, 
            IRequestHelper requestHelper, IConfiguration configuration) : base(configuration, requestHelper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _logger = logger;
            _transactionProducer = transactionProducer;
        }

        // api/transaction/
        [HttpPost(TransactionEndpoints.Deposit)]
        [SwaggerOperation(Summary = "Add deposit")]
        [SwaggerResponse(StatusCodes.Status201Created, "Deposit added", typeof(long))]
        public async Task<ActionResult<long>> AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Request to add Deposit in the controller");
            await CheckMicroservice(Microservice.MarvelousCrm);

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = await _transactionService.AddDeposit(transactionModel);

            _logger.LogInformation($"Deposit with id = {transactionId} added");
            var transactionForPublish = await _transactionService.GetTransactionById(transactionId);
            await _transactionProducer.NotifyTransactionAdded(transactionForPublish);

            return Ok(transactionId);
        }

        // api/transaction/
        [HttpPost(TransactionEndpoints.Transfer)]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Transfer successful", typeof(List<long>))]
        public async Task<ActionResult<List<long>>> AddTransfer([FromBody] TransferRequestModel transfer)
        {
            _logger.LogInformation("Request to add Transfer in the controller");
            await CheckMicroservice(Microservice.MarvelousCrm);

            var transferModel = _mapper.Map<TransferModel>(transfer);
            var transferIds = await _transactionService.AddTransfer(transferModel);

            _logger.LogInformation($"Transfer added");
            var transactionForPublishFirst = await _transactionService.GetTransactionById(transferIds[0]);
            var transactionForPublishSecond = await _transactionService.GetTransactionById(transferIds[1]);

            await _transactionProducer.NotifyTransactionAdded(transactionForPublishFirst);
            await _transactionProducer.NotifyTransactionAdded(transactionForPublishSecond);

            return Ok(transferIds);
        }

        [HttpPost(TransactionEndpoints.Withdraw)]
        [SwaggerOperation(Summary = "Withdraw")]
        [SwaggerResponse(StatusCodes.Status200OK, "Withdraw successful", typeof(long))]
        public async Task<ActionResult<long>> Withdraw([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Request to add Withdraw in the controller");
            await CheckMicroservice(Microservice.MarvelousCrm);

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            transactionModel.Type = TransactionType.Withdraw;
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Withdraw with id = {transactionId} added");
            var transactionForPublish = await _transactionService.GetTransactionById(transactionId);

            await _transactionProducer.NotifyTransactionAdded(transactionForPublish);

            return Ok(transactionId);
        }


        // api/Transactions/by-accountIds?accountIds=1&accountIds=2
        [HttpGet("by-accountIds")]
        [SwaggerOperation(Summary = "Get transactions by accountIds")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(List<ArrayList>))]
        public async Task<ActionResult<List<ArrayList>>> GetTransactionsByAccountIds(
            [FromQuery] List<int> accountIds)
        {
            _logger.LogInformation($"Request to receive all transactions by AccountIds in the controller");
            await CheckMicroservice(Microservice.MarvelousCrm);

            var transactionModels = await _transactionService.GetTransactionsByAccountIds(accountIds);
            var transactions = _mapper.Map<ArrayList>(transactionModels);

            _logger.LogInformation($"Transactions by AccountIds received");

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get transaction by id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successful", typeof(TransactionResponseModel))]
        public async Task<ActionResult<TransactionResponseModel>> GetTransactionById(long id)
        {
            _logger.LogInformation($"Request to receive transaction by Id = {id} in the controller");
            await CheckMicroservice(Microservice.MarvelousCrm);

            var transactionModel = await _transactionService.GetTransactionById(id);
            var transaction = _mapper.Map<TransactionResponseModel>(transactionModel);

            _logger.LogInformation($"Transactions by AccountId = {id} received");

            return Ok(transaction);
        }

        [HttpPost(TransactionEndpoints.ServicePayment)]
        [SwaggerOperation(Summary = "Service payment")]
        [SwaggerResponse(StatusCodes.Status200OK, "Payment successful", typeof(long))]
        public async Task<ActionResult<long>> ServicePayment([FromBody] TransactionRequestModel transaction)
        {
            _logger.LogInformation("Request to add Service payment in the conroller");
            await CheckMicroservice(Microservice.MarvelousResource);

            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            transactionModel.Type = TransactionType.ServicePayment;
            var transactionId = await _transactionService.Withdraw(transactionModel);

            _logger.LogInformation($"Service payment with Id = {transactionId} added");
            var transactionForPublish = await _transactionService.GetTransactionById(transactionId);
            await _transactionProducer.NotifyTransactionAdded(transactionForPublish);

            return Ok(transactionId);
        }
    }
}


//TransactionRequestModel
//TransferRequestModel
//ArrayList для антона вопрос