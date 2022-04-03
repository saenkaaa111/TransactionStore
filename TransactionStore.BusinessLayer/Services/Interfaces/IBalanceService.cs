using Marvelous.Contracts.Enums;

namespace TransactionStore.BusinessLayer.Services
{
    public interface IBalanceService
    {
        Task<decimal> GetAccountBalance(List<int> accountIds);
        Task<decimal> GetBalanceByAccountId(int accountId);
        Task<decimal> GetBalanceByAccountIds(List<int> accountIds);
        Task<decimal> GetBalanceByAccountIdsInGivenCurrency(List<int> accountIds, Currency currency);
    }
}