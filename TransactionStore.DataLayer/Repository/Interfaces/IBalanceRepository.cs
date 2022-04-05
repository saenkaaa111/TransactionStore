
namespace TransactionStore.DataLayer.Repository
{
    public interface IBalanceRepository
    {
        Task<decimal> GetBalanceByAccountId(int id);
    }
}