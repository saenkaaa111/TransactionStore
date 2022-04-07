using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        Task<long> AddTransaction(TransactionDto transaction);
        Task<List<long>> AddTransfer(TransferDto transaction);
        Task<TransactionDto> GetTransactionById(long id);
        Task<List<TransactionDto>> GetTransactionsByAccountIds(List<int> ids);
        Task<List<TransactionDto>> GetTransactionsByAccountIdsWithSecondHalfOfTransfer(List<int> ids);
    }
}