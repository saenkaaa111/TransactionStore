using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public ICurrencyRatesExchangeModel CurrencyRatesModel { get; set; }

        public Dictionary<string, decimal> SaveCurrencyRates(ICurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRatesModel = currencyRatesModel;
            return CurrencyRatesModel.Rates;
        }        
    }
}
