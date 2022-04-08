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
        private IMemoryCache _cache;
        private readonly ILogger<CalculationService> _logger;
        public const Currency BaseCurrency = Currency.USD;
        public const string Key = "CurrencyPairs";

        public CalculationService(ICurrencyRatesService currencyRates, IMemoryCache memoryCache,
            ILogger<CalculationService> logger)
        {
            _currencyRatesService = currencyRates;
            _cache = memoryCache;
            _logger = logger;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            var currencyRates = _currencyRatesService.CurrencyRates;

            if (currencyRates is null)
            {
                //currencyRates = _cache.Get<Dictionary<string, decimal>>(Key);

                string jsonread = File.ReadAllText("dictionary.json");
                currencyRates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonread);
            }
            else
            {
                //_cache.Set(Key, currencyRates);

                string json = JsonConvert.SerializeObject(currencyRates, Formatting.Indented);
                File.WriteAllText("dictionary.json", json);
            }

            currencyRates.TryGetValue($"{BaseCurrency}{currencyFrom}", out var currencyFromValue);
            currencyRates.TryGetValue($"{BaseCurrency}{currencyTo}", out var currencyToValue);

            if (currencyFrom == BaseCurrency)
                currencyFromValue = 1m;

            if (currencyTo == BaseCurrency)
                currencyToValue = 1m;

            if (currencyFromValue == 0 || currencyToValue == 0)
                throw new CurrencyNotReceivedException("The request for the currency value was not received");

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 2);

            _logger.LogInformation("Currency converted");

            return convertAmount;
         }
    }
}
