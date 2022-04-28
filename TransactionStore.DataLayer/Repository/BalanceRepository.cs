using Dapper;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data;

namespace TransactionStore.DataLayer.Repository
{
    public class BalanceRepository : BaseRepository, IBalanceRepository
    {
        private const string _transactionGetAccountBalance = "dbo.Transaction_GetAccountBalance";
        private const string _transactionGetLastDate = "dbo.Transaction_GetLastDate";
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
        
        public DateTime GetLastDate()
        {
            _logger.LogInformation("Connecting to the database");
            using IDbConnection connection = Connection;
            _logger.LogInformation("Connection made");

            var dateFromBD = connection.QueryFirstOrDefault<DateTime>(
                _transactionGetLastDate,
                commandType: CommandType.StoredProcedure
                );

            _logger.LogInformation($"Date last transaction recieved");
            return dateFromBD;
        }
    }
}
