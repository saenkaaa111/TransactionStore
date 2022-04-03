using Marvelous.Contracts.Enums;
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

        public CalculationService(ICurrencyRatesService currencyRates, ILogger<CalculationService> logger)
        {
            _logger = logger;
            _currencyRatesService = currencyRates;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.LogInformation($"Request to convert currency from {currencyFrom} to {currencyTo}");
            var rates = _currencyRatesService.Pairs;

            if (rates == null)
            {
                string jsonread = File.ReadAllText("dictionary.json");
                rates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonread);
            }
            else
            {
                string json = JsonConvert.SerializeObject(rates, Formatting.Indented);
                File.WriteAllText("dictionary.json", json);
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

            _logger.LogInformation("Curency converted");

            return convertAmount;
        }
    }
}
