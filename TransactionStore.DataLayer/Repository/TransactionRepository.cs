using Dapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Exceptions;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddDepositProcedure = "dbo.Transaction_InsertDeposit";
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        private const string _transactionGetByIdProcedure = "dbo.Transaction_SelectById";
        private const string _transactionTransfer = "dbo.Transaction_Transfer";
        private const string _transactionGetByAccountIdsProcedure = "dbo.Transaction_SelectByAccountIds";
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(IDbConnection dbConnection, ILogger<TransactionRepository> logger) : base(dbConnection)
        {
            _logger = logger;
        }

        public async Task<long> AddDeposit(TransactionDto transaction)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");
            var id = await connection.QueryFirstOrDefaultAsync<long>(
                _transactionAddDepositProcedure,
                new
                {
                    transaction.Amount,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency
                },
                commandType: CommandType.StoredProcedure
                );

            _logger.LogInformation($"{transaction.Type} with id = {id} added in database");

            return id;
        }
        
        public async Task<long> AddTransaction(TransactionDto transaction, DateTime lastTransactionDate)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");
            try
            {
                var id = await connection.QueryFirstOrDefaultAsync<long>(
                _transactionAddProcedure,
                new
                {
                    transaction.Amount,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency,
                    Date = lastTransactionDate
                },
                commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation($"{transaction.Type} with id = {id} added in database");

                return id;
            }
            catch (SqlException ex)
            {
                _logger.LogError("Error: Flood crossing");
                throw new TransactionsConflictException("Flood crossing, try again");                
            }                    
        }

        public async Task<List<long>> AddTransfer(TransferDto transaction, DateTime lastTransactionDate)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            try
            {
                var (transactionIdFrom, transactionIdTo) = await connection.QueryFirstOrDefaultAsync<(long, long)>(
                _transactionTransfer,
                new
                {
                    Amount = transaction.Amount * (-1),
                    ConvertedAmount = transaction.ConvertedAmount,
                    AccountIdFrom = transaction.AccountIdFrom,
                    AccountIdTo = transaction.AccountIdTo,
                    CurrencyFrom = transaction.CurrencyFrom,
                    CurrencyTo = transaction.CurrencyTo,
                    type = TransactionType.Transfer
                },
                commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation($"Transfers with ids = { transactionIdFrom},{ transactionIdTo } added in database");
                var listId = new List<long>() { transactionIdFrom, transactionIdTo };
                return listId;
            }

            catch (SqlException ex)
            {
                _logger.LogError("Error: Flood crossing");
                throw new Exception("Flood crossing, try again");
            }
        }

        public async Task<List<TransactionDto>> GetTransactionsByAccountIds(List<int> ids)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            var tvpTable = new DataTable();
            tvpTable.Columns.Add(new DataColumn("AccountId", typeof(int)));
            ids.ForEach(id => tvpTable.Rows.Add(id));

            var listTransactions = (await connection.QueryAsync<TransactionDto>(
                    _transactionGetByAccountIdsProcedure,
                    new { tvp = tvpTable.AsTableValuedParameter("[dbo].[AccountTVP]") },
                    commandType: CommandType.StoredProcedure
               ))
               .ToList();

            _logger.LogInformation($"Transactions by  AccountId = {ids} recieved");

            return listTransactions;
        }

        public async Task<TransactionDto> GetTransactionById(long id)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            var transactionDto = await connection.QuerySingleAsync<TransactionDto>(
                _transactionGetByIdProcedure, new { Id = id },
                commandType: CommandType.StoredProcedure);

            _logger.LogInformation($"Transaction by Id = {id} recieved");

            return transactionDto;
        }
    }
}