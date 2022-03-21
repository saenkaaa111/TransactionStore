﻿using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using TransactionStore.DataLayer.Entities;
using Marvelous.Contracts;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        private const string _transactionGetByAccountIdProcedure = "dbo.Transaction_SelectByAccountId";
        private const string _transactionGetByAccountIdsProcedure = "dbo.Transaction_SelectByAccountIds";
        private const string _transactionGetByIdProcedure = "dbo.Transaction_SelectById";
        private const string _transactionTransfer = "dbo.Transaction_Transfer";
        private const string _transactionGetAccountBalance = "dbo.Transaction_GetAccountBalance";
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(IDbConnection dbConnection, ILogger<TransactionRepository> logger) : base(dbConnection)
        {
            _logger = logger;
        }

        public async Task<long> AddTransaction(TransactionDto transaction)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            var id = await connection.QueryFirstOrDefaultAsync<long>(
                _transactionAddProcedure,
                new
                {
                    transaction.Amount,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency
                },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation($"Транзакция типа {transaction.Type} с id = {id} добавлена в БД.");
            return id;
        }

        public async Task<List<long>> AddTransfer(TransferDto transaction)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            var listId = await connection.QueryFirstOrDefaultAsync<dynamic>(
            _transactionTransfer,
                new
                {
                    amount = transaction.Amount * (-1),
                    transaction.ConvertedAmount,
                    transaction.AccountIdFrom,
                    transaction.AccountIdTo,
                    transaction.CurrencyFrom,
                    transaction.CurrencyTo,
                    type = TransactionType.Transfer

                },
                commandType: CommandType.StoredProcedure
                ) as IDictionary<string, object>;
            _logger.LogInformation($"Транзакция типа Transfer с id = {(long)listId.Values.First()}, {(long)listId.Values.Last()} добавлены в БД.");

            return new List<long>() { (long)listId.Values.First(), (long)listId.Values.Last() };

        }

        public async Task<List<TransactionDto>> GetTransactionsByAccountId(long id)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            var listTransactions = (await connection.QueryAsync<TransactionDto>(
                _transactionGetByAccountIdProcedure,
                new { AccountId = id },
                commandType: CommandType.StoredProcedure
            )).ToList();

            _logger.LogInformation($"Транзакция по AccountId = {id} получены.");

            return listTransactions;
        }

        public async Task<List<TransactionDto>> GetTransactionsByAccountIds(List<long> accountIds)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            var tvpTable = new DataTable();
            tvpTable.Columns.Add(new DataColumn("AccountId", typeof(long)));
            accountIds.ForEach(id => tvpTable.Rows.Add(id));

            var listTransactions = (await connection.QueryAsync<TransactionDto>(
                    _transactionGetByAccountIdsProcedure,
                    new { tvp = tvpTable.AsTableValuedParameter("[dbo].[AccountTVP]") },
                    commandType: CommandType.StoredProcedure
               ))
               .ToList();

            _logger.LogInformation($"Транзакция по AccountIds = {accountIds} получены.");

            return listTransactions;
        }

        public async Task<TransactionDto> GetTransactionById(long id)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            var transactionDto = await connection.QuerySingleAsync<TransactionDto>(
                _transactionGetByIdProcedure, new { Id = id },
                commandType: CommandType.StoredProcedure);

            _logger.LogInformation($"Транзакция по Id = {id} получена.");

            return transactionDto;
        }

        public async Task<decimal> GetAccountBalance(long id)
        {
            _logger.LogInformation("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Подключение произведено.");

            return await connection.QuerySingleAsync<decimal>(
                _transactionGetAccountBalance,
                 new { Id = id },
                commandType: CommandType.StoredProcedure
                );

        }
    }
}