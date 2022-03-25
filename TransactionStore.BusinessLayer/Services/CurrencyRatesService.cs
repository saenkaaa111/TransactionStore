using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public Dictionary<string,decimal> Pairs { get; set; }

        public void SaveCurrencyRates(ICurrencyRatesExchangeModel currencyRatesModel)
        {
            Pairs = currencyRatesModel.Rates;            
        }        
    }
}
