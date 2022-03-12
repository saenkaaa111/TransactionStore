using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICurrencyRates
    {
        public Dictionary<Currency, decimal> Rates { get; }
    }
}