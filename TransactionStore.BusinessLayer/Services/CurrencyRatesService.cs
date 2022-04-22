using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Microsoft.Extensions.Caching.Memory;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        private IMemoryCache _cache;
        public const string Key = "CurrencyPairs";

        public Dictionary<string, decimal> CurrencyRates { get; set; }
        public Currency BaseCurrency { get; set; }

        public CurrencyRatesService(IMemoryCache memoryCache) { _cache = memoryCache; }

        public void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRates = currencyRatesModel.Rates;
        }

        public void SaveBaseCurrency(CurrencyRatesExchangeModel currencyRatesModel)
        {
            BaseCurrency = currencyRatesModel.BaseCurrency;
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
