﻿using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Services.Interfaces
{
    public interface ITransactionService
    {
        int AddDeposit(TransactionModel transactionModel);
    }
}
