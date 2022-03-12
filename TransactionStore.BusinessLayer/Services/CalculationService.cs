using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ICurrencyRates _currencyRates;
        public CalculationService(ICurrencyRates currencyRates)
        {
            _currencyRates = currencyRates;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            var rates = _currencyRates.Rates;
            rates.TryGetValue(currencyFrom, out var currencyFromValue);
            rates.TryGetValue(currencyTo, out var currencyToValue);

            return decimal.Round((currencyToValue / currencyFromValue * amount), 3);
        }

        public decimal GetAccountBallance()
        {
            return 1m;
        }
    }
}
