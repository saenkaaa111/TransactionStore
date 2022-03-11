using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDto transaction);
        List<TransactionDto> GetByAccountId(int id);
        List<TransactionDto> GetTransactionsByAccountIds(List<int> accountIds);
        public TransactionDto GetTransactionById(int id);
    }
}