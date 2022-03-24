using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        Task<long> AddTransaction(TransactionDto transaction);
        Task<List<long>> AddTransfer(TransferDto transaction);
        Task<decimal> GetAccountBalance(long id);
        Task<TransactionDto> GetTransactionById(long id);
        Task<List<TransactionDto>> GetTransactionsByAccountId(long id);
        Task<List<TransactionDto>> GetTransactionsByAccountIdMinimal(long id);
        Task<List<TransactionDto>> GetTransactionsByAccountIds(List<long> accountIds);
    }
}