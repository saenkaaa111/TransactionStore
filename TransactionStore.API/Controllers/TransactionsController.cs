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
        [SwaggerResponse(201, "Transaction added")]
        public ActionResult AddDeposit([FromBody] TransactionRequestModel transaction)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddDeposit(transactionModel);

            return StatusCode(201, transactionId);
        }


        // api/transaction/
        [HttpPost("transfer-to-{accountId}")]
        [SwaggerOperation(Summary = "Add transfer")]
        [SwaggerResponse(201, "Transaction added")]
        public ActionResult AddTransfer([FromBody] TransactionRequestModel transaction, int accountId)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddTransfer(transactionModel, accountId);

            return StatusCode(201, transactionId);
        }
    }
}
