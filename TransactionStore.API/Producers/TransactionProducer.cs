using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Producers
{
    public class TransactionProducer : ITransactionProducer
    {
        private readonly ITransactionService _transactionService;
        private readonly ICalculationService _calculationService;
        private readonly ILogger<TransactionProducer> _logger;
        private readonly IBus _bus;

        public TransactionProducer(ITransactionService transactionService, ICalculationService calculationService,
            ILogger<TransactionProducer> logger, IBus bus)
        {
            _transactionService = transactionService;
            _calculationService = calculationService;
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyTransactionAdded(long id)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var transaction = await _transactionService.GetTransactionById(id);

            await _bus.Publish<TransactionExchangeModel>(new
            {
                transaction.Id,
                transaction.Amount,
                transaction.Date,
                transaction.AccountId,
                transaction.Type,
                transaction.Currency,
                RubRate = _calculationService.ConvertCurrency(transaction.Currency, Currency.RUB, 1)
            },
            source.Token);

            _logger.LogInformation("Published");
        }
    }
}
