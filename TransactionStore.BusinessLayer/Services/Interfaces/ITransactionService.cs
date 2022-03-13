using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services.Interfaces
{
    public interface ITransactionService
    {
        int AddDeposit(TransactionModel transactionModel);
        List<int> AddTransfer(TransferModel transactionModel);
        int Withdraw(TransactionModel transactionModel);
        List<TransactionModel> GetByAccountId(int id);
        List<TransactionModel> GetTransactionsByAccountIds(List<int> accountIds);
        public TransactionModel GetTransactionById(int id);
    }
}
