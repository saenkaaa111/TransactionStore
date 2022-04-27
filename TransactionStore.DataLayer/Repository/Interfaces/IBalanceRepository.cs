
using System.Collections;

namespace TransactionStore.DataLayer.Repository
{
    public interface IBalanceRepository
    {
        Task<(decimal, DateTime)> GetBalanceByAccountId(int accountId);
        DateTime GetLastDate();
    }
}