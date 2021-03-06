using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Consumers
{
    public class CurrencyRatesConsumer : IConsumer<CurrencyRatesExchangeModel>
    {
        private readonly ICurrencyRatesService _currencyRatesService;
        private readonly ILogger<CurrencyRatesConsumer> _logger;

        public CurrencyRatesConsumer(ICurrencyRatesService currencyRatesService,
            ILogger<CurrencyRatesConsumer> logger)
        {
            _currencyRatesService = currencyRatesService;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<CurrencyRatesExchangeModel> context)
        {
            _currencyRatesService.SaveCurrencyRates(context.Message);
            _logger.LogInformation("CurrencyRatesExchangeModel recieved");

            return Task.CompletedTask;
        }
    }
}
