using Marvelous.Contracts;
using NLog;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ICurrencyRates _currencyRates;
        private static Logger _logger;

        public CalculationService(ICurrencyRates currencyRates)
        {
            _currencyRates = currencyRates;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.Debug($"Запрос на конвертацию валюты с {currencyFrom} в {currencyTo} ");

            var rates = _currencyRates.Rates;
            rates.TryGetValue(currencyFrom, out var currencyFromValue);            
            rates.TryGetValue(currencyTo, out var currencyToValue);

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 4);
            
            _logger.Debug("Валюта конвертирована");
            return convertAmount;
        }

        public decimal GetAccountBallance()
        {
            return 1m;
        }
    }
}
