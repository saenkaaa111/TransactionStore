using Marvelous.Contracts.Enums;

namespace TransactionStore.BusinessLayer.Services
{
    public interface IBalanceService
    {
        Task<decimal> GetBalanceByAccountIdsInGivenCurrency(List<int> accountIds, Currency currency);
    }
}