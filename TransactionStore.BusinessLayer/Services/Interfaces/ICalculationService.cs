using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        Task<decimal> ConvertCurrency(string currencyFrom, string currencyTo, decimal amount);
        Task<decimal> GetAccountBalance(List<long> accauntId);
    }
}