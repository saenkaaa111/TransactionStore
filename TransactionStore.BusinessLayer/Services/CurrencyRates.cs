using Marvelous.Contracts;
using TransactionStore.BusinessLayer.Services.Interfaces;

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
                { "USDRSD", 106m }
            };
        }
    }
}
