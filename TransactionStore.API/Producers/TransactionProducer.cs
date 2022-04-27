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
            await _bus.Publish(new TransactionExchangeModel
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                AccountId = transaction.AccountId,
                Type = transaction.Type,
                Currency = transaction.Currency,
                RubRate = _calculationService.ConvertCurrency(transaction.Currency, Currency.RUB, 1)
            },
            source.Token);

            _logger.LogInformation($"Transaction with id = {transaction.Id} published");
        }
    }
}