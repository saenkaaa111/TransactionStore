using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ITransactionService
    {
        Task<long> AddDeposit(TransactionModel transactionModel);
        Task<List<long>> AddTransfer(TransferModel transferModel);
        Task<TransactionModel> GetTransactionById(long id);
        Task<ArrayList> GetTransactionsByAccountIds(List<int> ids);
        Task<long> Withdraw(TransactionModel transactionModel);
    }
}