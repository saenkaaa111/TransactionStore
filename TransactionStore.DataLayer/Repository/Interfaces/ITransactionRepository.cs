using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        Task<long> AddDeposit(TransactionDto transaction);
        Task<long> AddTransaction(TransactionDto transaction, DateTime lastTransactionDate);
        Task<List<long>> AddTransfer(TransferDto transaction, DateTime lastTransactionDate);
        Task<TransactionDto> GetTransactionById(long id);
        Task<List<TransactionDto>> GetTransactionsByAccountIds(List<int> ids);
    }
}