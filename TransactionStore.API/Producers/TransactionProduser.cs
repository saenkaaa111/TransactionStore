using MassTransit;
using TransactionStore.BusinessLayer.Services;
using Marvelous.Contracts.Models.ExchangeModels;
using TransactionStore.API.Models;
using Marvelous.Contracts;
using TransactionStore.API.Consumers;

namespace TransactionStore.API.Producers
{
    public class TransactionProduser
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<CurrencyRatesConsumer> _logger;

        public TransactionProduser(ITransactionService transactionService,
            ILogger<CurrencyRatesConsumer> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }


        public async Task NotifyTransactionAdded(IPublishEndpoint publishEndpoint, long id)
        {
            //var transaction = _transactionService.GetTransactionById(id);
            await publishEndpoint.Publish<TransactionResponseModel>(new 
            {
                id = id,
                Amount = 345,
                Date = DateTime.UtcNow,
                AccountId = 56,
                Type = 2,
                Currency =102

            });
            _logger.LogInformation("Send Transaction");

        }
    }
}
