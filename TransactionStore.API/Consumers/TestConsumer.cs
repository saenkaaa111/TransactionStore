using A;
using MassTransit;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Consumers
{
    public class TestConsumer: IConsumer<ValueEntered>
    {
        private readonly ICurrencyRatesService _currencyRatesService;
        private readonly ILogger<CurrencyRatesConsumer> _logger;

        public TestConsumer(ICurrencyRatesService currencyRatesService,
            ILogger<CurrencyRatesConsumer> logger)
        {
            _currencyRatesService = currencyRatesService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            await context.Publish<ValueEntered>(new
            {
                context.Message.Value
            });


        }
    }
}
