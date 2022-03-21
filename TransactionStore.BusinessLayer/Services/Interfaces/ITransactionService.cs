using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services
{
    public interface ITransactionService
    {
        int AddDeposit(TransactionModel transactionModel);
        List<int> AddTransfer(TransferModel transactionModel);
        decimal GetBalanceByAccountId(int accountId);
        decimal GetBalanceByAccountIds(List<int> accountId);
        TransactionModel GetTransactionById(int id);
        ArrayList GetTransactionsByAccountId(int id);
        List<TransactionModel> GetTransactionsByAccountIds(List<int> accountIds);
        int Withdraw(TransactionModel transactionModel);
    }
}