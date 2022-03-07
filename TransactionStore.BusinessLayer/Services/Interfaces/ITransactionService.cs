using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services.Interfaces
{
    public interface ITransactionService
    {
        int AddDeposit(TransactionModel transactionModel);
        List<int> AddTransfer(TransactionModel transactionModel, int accountIdTo, int currencyTo);
    }
}
