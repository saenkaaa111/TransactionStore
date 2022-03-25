using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Consumers
{
    public class CurrencyRatesConsumer : IConsumer<ICurrencyRatesExchangeModel>
    {
        private readonly ICurrencyRatesService _currencyRatesService;
        private readonly ILogger<CurrencyRatesConsumer> _logger;

        public CurrencyRatesConsumer(ICurrencyRatesService currencyRatesService,
            ILogger<CurrencyRatesConsumer> logger)
        {
            _currencyRatesService = currencyRatesService;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ICurrencyRatesExchangeModel> context)
        {
            _currencyRatesService.SaveCurrencyRates(context.Message);
            _logger.LogInformation("Getting CurrencyRatesExchangeModel");

            return Task.CompletedTask;
        }
    }
}
