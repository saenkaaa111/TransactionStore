
namespace TransactionStore.DataLayer.Repository
{
    public interface IBalanceRepository
    {
        Task<decimal> GetAccountBalance(int id);
    }
}