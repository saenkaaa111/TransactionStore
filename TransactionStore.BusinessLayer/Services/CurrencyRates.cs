using Marvelous.Contracts;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRates : ICurrencyRates
    {
        public Dictionary<string, decimal> GetRates()
        {
            return new Dictionary<string, decimal>
            {
                { "USDRUB", 105m },
                { "USDEUR", 0.9m },
                { "USDJPY", 118m },
                { "USDCNY", 6m },
                { "USDTRY", 14m },
                { "USDRSD", 106m },
                { "RUBUSD", 0.009m },
                { "EURUSD", 1.11m },
                { "JPYUSD", 0.008m },
                { "CNYUSD", 0.16m },
                { "TRYUSD", 0.068m },
                { "RSDUSD", 2.65m },

            };
        }

    }
}
