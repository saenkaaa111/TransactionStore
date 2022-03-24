using Marvelous.Contracts;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Producers
{
    public class TransactionProduser 
    {
        private readonly ITransactionService _transactionService;
        private readonly ICalculationService _calculationService;
        private readonly ILogger<TransactionProduser> _logger;

        public TransactionProduser(ITransactionService transactionService, ICalculationService calculationService,
            ILogger<TransactionProduser> logger)
        {
            _transactionService = transactionService;
            _calculationService = calculationService;
            _logger = logger;
        }

        public async Task NotifyTransactionAdded(IPublishEndpoint publishEndpoint, long id)
        {
            var transaction = await _transactionService.GetTransactionById(id);

            await publishEndpoint.Publish<TransactionExchangeModel>(new TransactionExchangeModel
            {
                transaction.Id,
                transaction.Amount,
                transaction.Date,
                transaction.AccountId,
                transaction.Type,
                transaction.Currency,
                RubRate = _calculationService.ConvertCurrency(transaction.Currency, Currency.RUB, 1)
            });

            _logger.LogInformation("Transaction published");
        }
    }
}
