﻿using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDto transaction);
    }
}