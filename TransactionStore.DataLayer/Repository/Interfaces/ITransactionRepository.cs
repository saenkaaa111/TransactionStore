using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDto transaction);
        List<TransactionDto> GetByAccountId(int id);
        List<TransactionDto> GetByAccountIds(List<int> accountIds);
    }
}