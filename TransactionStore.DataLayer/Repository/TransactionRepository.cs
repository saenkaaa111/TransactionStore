﻿using Dapper;
using System.Data;
using System.Data.SqlClient;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private const string _connectionString = "Data Source = 80.78.240.16; Database=Transaction;User Id = student; Password=qwe!23;";
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        
        public int AddTransaction(TransactionDto transaction)
        {
            using var connection = new SqlConnection(_connectionString);

            return connection.QueryFirstOrDefault<int>(
                    _transactionAddProcedure,
                    new
                    {
                        transaction.Amount,
                        transaction.Date,
                        transaction.AccountId,
                        transaction.Type
                    },
                    commandType: CommandType.StoredProcedure
                );
        }    
    }
}
