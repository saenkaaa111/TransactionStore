using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public class CurrencyRatesService : ICurrencyRatesService
    {
        public Dictionary<string, decimal> CurrencyRates { get; set; }

        public void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel)
        {
            CurrencyRates = currencyRatesModel.Rates;
        }
    }
}
