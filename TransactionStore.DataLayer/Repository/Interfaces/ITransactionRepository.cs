using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDto transaction);
        List<int> AddTransfer(TransferDto transaction);
        List<TransactionDto> GetByAccountId(int id);
        List<TransactionDto> GetTransactionsByAccountIds(List<int> accountIds);
        public TransactionDto GetTransactionById(int id);
    }
}