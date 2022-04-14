using Marvelous.Contracts.ExchangeModels;
using Microsoft.Extensions.Caching.Memory;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public Dictionary<string, decimal> CurrencyRates { get; set; }
        private IMemoryCache _cache;
        public const string Key = "CurrencyPairs";

        public CurrencyRatesService(IMemoryCache memoryCache) { _cache = memoryCache; }
        public CurrencyRatesService() {}

        public void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRates = currencyRatesModel.Rates;
            
        }

        public Dictionary<string, decimal> GetCurrencyRates()
        {
            if (CurrencyRates is null)
            {
                CurrencyRates = _cache.Get<Dictionary<string, decimal>>(Key);

            }
            else
            {
                _cache.Set(Key, CurrencyRates);

            }
            return CurrencyRates;
        }
        
    }
}
