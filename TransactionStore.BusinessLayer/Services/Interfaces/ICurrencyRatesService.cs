using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRatesService
    {
        Dictionary<string, decimal> CurrencyRates { get; protected set; }
        public Currency BaseCurrency { get; set; }
        void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel);
        public void SaveBaseCurrency(CurrencyRatesExchangeModel currencyRatesModel);
        Dictionary<string, decimal> GetCurrencyRates();
    }
}