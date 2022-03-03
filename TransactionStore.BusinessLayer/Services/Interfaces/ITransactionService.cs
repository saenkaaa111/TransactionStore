using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services.Interfaces
{
    public interface ITransactionService
    {
        int AddTransaction(TransactionModel transactionModel);
    }
}
