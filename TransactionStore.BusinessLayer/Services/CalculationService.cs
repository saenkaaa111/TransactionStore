using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TransactionStore.BusinessLayer.Exceptions;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private ICurrencyRatesService _currencyRatesService;
        private IMemoryCache _cache;
        private readonly ILogger<CalculationService> _logger;
        public const Currency BaseCurrency = Currency.USD;

        public CalculationService(ICurrencyRatesService currencyRates, IMemoryCache memoryCache,
            ILogger<CalculationService> logger)
        {
            _currencyRatesService = currencyRates;
            _cache = memoryCache;
            _logger = logger;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            var rates = _currencyRatesService.Rates;

            if (rates is null)
            {
                _cache.Get(rates);
            }
            else
            {
                _cache.Remove(rates);
                _cache.CreateEntry(rates);
            }

            rates.TryGetValue($"{BaseCurrency}{currencyFrom}", out var currencyFromValue);
            rates.TryGetValue($"{BaseCurrency}{currencyTo}", out var currencyToValue);

            if (currencyFrom == BaseCurrency)
                currencyFromValue = 1m;

            if (currencyTo == BaseCurrency)
                currencyToValue = 1m;

            if (currencyFromValue == 0 || currencyToValue == 0)
                throw new CurrencyNotReceivedException("The request for the currency value was not received");

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 2);

            return convertAmount;
        }
    }
}
