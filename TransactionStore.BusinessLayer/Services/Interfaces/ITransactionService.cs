using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ITransactionService
    {
        Task<long> AddDeposit(TransactionModel transactionModel);
        Task<List<long>> AddTransfer(TransferModel transactionModel);
        Task<decimal> GetBalanceByAccountId(long accountId);
        Task<decimal> GetBalanceByAccountIds(List<long> accountId);
        Task<TransactionModel> GetTransactionById(long id);
        Task<List<TransactionModel>> GetTransactionsByAccountIds(List<long> accountIds);
        Task<long> Withdraw(TransactionModel transactionModel);
        ArrayList GetTransactionsByAccountId(int id);
    }
}