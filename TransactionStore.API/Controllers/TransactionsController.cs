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
        [HttpPost]
        [SwaggerOperation(Summary = "Add transaction")]
        [SwaggerResponse(201, "Transaction added")]
        public ActionResult AddTransaction([FromBody] TransactionRequestModel transaction)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);
            var transactionId = _transactionService.AddTransaction(transactionModel);

            return StatusCode(201, transactionId);
        }
    }
}
