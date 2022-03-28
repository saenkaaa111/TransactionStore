using Marvelous.Contracts.Enums;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount);
        Task<decimal> GetAccountBalance(List<int> accountId);
    }
}