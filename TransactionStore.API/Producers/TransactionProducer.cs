using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Producers
{
    public class TransactionProducer : ITransactionProducer
    {
        private readonly ICalculationService _calculationService;
        private readonly ILogger<TransactionProducer> _logger;
        private readonly IBus _bus;

        public TransactionProducer(ICalculationService calculationService,
            ILogger<TransactionProducer> logger, IBus bus)
        {
            _calculationService = calculationService;
            _logger = logger;
            _bus = bus;
        }

        public async Task NotifyTransactionAdded(TransactionModel transaction)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
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