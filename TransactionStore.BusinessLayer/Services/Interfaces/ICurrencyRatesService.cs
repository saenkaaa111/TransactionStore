using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRatesService
    {
        void SaveCurrencyRates(CurrencyRatesExchangeModel currencyRatesModel);
        Dictionary<string, decimal> Pairs { get; protected set; }
    }
}