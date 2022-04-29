using Dapper;
using Microsoft.Extensions.Logging;
using System.Collections;
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

        public async Task<(decimal, DateTime)> GetBalanceByAccountId(int accountId)
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            var balance = connection.QueryFirstOrDefault<(decimal, DateTime) >(
                _transactionGetAccountBalance,
                 new { Id = accountId },
                commandType: CommandType.StoredProcedure
                );

            _logger.LogInformation($"Balance by AccountId recieved");
            return balance;
        }
    }
}
