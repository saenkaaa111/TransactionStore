using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TransactionStore.BusinessLayer.Exceptions;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private ICurrencyRatesService _currencyRatesService;
        private readonly ILogger<CalculationService> _logger;
        public const Currency BaseCurrency = Currency.USD;
        public const string Key = "CurrencyPairs";

        public CalculationService(ICurrencyRatesService currencyRates, ILogger<CalculationService> logger)
        {
            _currencyRatesService = currencyRates;
            _logger = logger;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            var currencyRates = _currencyRatesService.GetCurrencyRates();

            currencyRates.TryGetValue($"{BaseCurrency}{currencyFrom}", out var currencyFromValue);
            currencyRates.TryGetValue($"{BaseCurrency}{currencyTo}", out var currencyToValue);

            if (currencyFrom == BaseCurrency)
                currencyFromValue = 1m;

            if (currencyTo == BaseCurrency)
                currencyToValue = 1m;

            if (currencyFromValue == 0 || currencyToValue == 0)
            {
                _logger.LogError("Exception: The request for the currency value was not received");
                throw new CurrencyNotReceivedException("The request for the currency value was not received");
            }
            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 2);

            _logger.LogInformation("Currency converted");

            return convertAmount;
         }
    }
}
