using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ITransactionService
    {
        Task<long> AddDeposit(TransactionModel transactionModel);
        Task<List<long>> AddTransfer(TransferModel transactionModel);
        Task<TransactionModel> GetTransactionById(long id);
        Task<List<TransactionModel>> GetTransactionsByAccountIds(List<int> accountIds);
        Task<long> Withdraw(TransactionModel transactionModel);
        Task<ArrayList> GetTransactionsByAccountId(int id);
    }
}