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

        public TransactionProducer(ITransactionService transactionService, ICalculationService calculationService,
            ILogger<TransactionProducer> logger)
        {
            _transactionService = transactionService;
            _calculationService = calculationService;
            _logger = logger;
        }

        public async Task Main(long id)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq://80.78.240.16", hst =>
                {
                    hst.Username("nafanya");
                    hst.Password("qwe!23");
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);

            try
            {
                var transaction = await _transactionService.GetTransactionById(id);

                await busControl.Publish<TransactionExchangeModel>(new
                {
                    transaction.Id,
                    transaction.Amount,
                    transaction.Date,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency,
                    RubRate = _calculationService.ConvertCurrency(transaction.Currency, Currency.RUB, 1)
                });

                _logger.LogInformation("Published");
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
