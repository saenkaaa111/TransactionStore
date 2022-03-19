using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ITransactionService
    {
        Task<long> AddDeposit(TransactionModel transactionModel);
        List<int> AddTransfer(TransferModel transactionModel);
        Task<long> Withdraw(TransactionModel transactionModel);
        List<TransactionModel> GetTransactionsByAccountId(int id);
        Task<List<TransactionModel>> GetTransactionsByAccountIds(List<long> accountIds);
        Task<TransactionModel> GetTransactionById(long id);
        decimal GetBalanceByAccountId(int accountId);
        decimal GetBalanceByAccountIds(List<int> accountId);
    }
}
