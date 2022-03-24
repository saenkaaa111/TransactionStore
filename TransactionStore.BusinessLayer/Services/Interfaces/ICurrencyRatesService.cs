using Marvelous.Contracts.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRatesService
    {
        Dictionary<string, decimal> SaveCurrencyRates(ICurrencyRatesExchangeModel currencyRates);
    }
}