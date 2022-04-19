
using System.Collections;

namespace TransactionStore.DataLayer.Repository
{
    public interface IBalanceRepository
    {
        Task<ArrayList> GetBalanceByAccountId(int id);
        DateTime GetLastDate();
    }
}