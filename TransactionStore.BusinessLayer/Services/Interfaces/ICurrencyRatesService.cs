
using Marvelous.Contracts.Models.ExchangeModels;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRatesService
    {
        Dictionary<string, decimal> SaveCurrencyRates(CurrencyRatesExchangeModel currencyRates);
    }
}