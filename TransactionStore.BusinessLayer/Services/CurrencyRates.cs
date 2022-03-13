using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRates : ICurrencyRates
    {
        public Dictionary<Currency, decimal> Rates { get; } = new Dictionary<Currency, decimal>
        {
            { Currency.USD, 1m },
            { Currency.RUB, 116m },
            { Currency.EUR, 0.9m },
            { Currency.CNY, 6.3m },
            { Currency.GBP, 0.7m },
        };
    }
}
