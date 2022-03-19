using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        Task<long> AddTransaction(TransactionDto transaction);
        List<int> AddTransfer(TransferDto transfer);
        List<TransactionDto> GetTransactionsByAccountId(int id);
        Task<List<TransactionDto>> GetTransactionsByAccountIds(List<long> accountIds);
        Task<TransactionDto> GetTransactionById(long id);
        decimal GetAccountBalance(int id);
    }
}