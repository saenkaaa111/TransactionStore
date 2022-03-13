using Marvelous.Contracts;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ICalculationService
    {
        decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount);
        decimal GetAccountBalance(int accauntId);
    }
}