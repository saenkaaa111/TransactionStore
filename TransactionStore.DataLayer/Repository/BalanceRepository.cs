﻿using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace TransactionStore.DataLayer.Repository
{
    public class BalanceRepository : BaseRepository, IBalanceRepository
    {
        private const string _transactionGetAccountBalance = "dbo.Transaction_GetAccountBalance";
        private readonly ILogger<BalanceRepository> _logger;

        public BalanceRepository(IDbConnection dbConnection, ILogger<BalanceRepository> logger) : base(dbConnection)
        {
            _logger = logger;
        }

        public async Task<decimal> GetAccountBalance(int id)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            var balance = await connection.QuerySingleAsync<decimal>(
                _transactionGetAccountBalance,
                 new { Id = id },
                commandType: CommandType.StoredProcedure
                );

            _logger.LogInformation($"Balance by AccountId recieved");

            return balance;
        }
    }
}