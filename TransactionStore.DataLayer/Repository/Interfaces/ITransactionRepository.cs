﻿using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public interface ITransactionRepository
    {
        int AddTransaction(TransactionDto transaction);
        DateTime AddTransferFrom(TransactionDto transaction);
        int AddTransferTo(TransactionDto transaction);
        List<TransactionDto> GetByAccountId(int id);
        List<TransactionDto> GetTransactionsByAccountIds(List<int> accountIds);
    }
}