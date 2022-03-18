using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount);
        decimal GetAccountBalance(int accauntId);
    }
}