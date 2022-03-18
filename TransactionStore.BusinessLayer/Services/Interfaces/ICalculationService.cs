using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal GetAccountBalance(List<int> accauntId);
        decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount);
    }
}