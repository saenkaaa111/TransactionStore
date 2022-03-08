using Dapper;
using System.Data;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        private const string _transactionGetByAccountIdProcedure = "dbo.Transaction_SelectByAccountId";
        private const string _transactionGetByAccountIdsProcedure = "dbo.Transaction_SelectByAccountIds";

        public TransactionRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public int AddTransaction(TransactionDto transaction)
        {
            return _connection.QueryFirstOrDefault<int>(
                    _transactionAddProcedure,
                    new
                    {
                        transaction.Amount,
                        transaction.Date,
                        transaction.AccountId,
                        transaction.Type,
                        transaction.Currency
                    },
                    commandType: CommandType.StoredProcedure
                );
        }

        public List<TransactionDto> GetByAccountId(int id)
        {
            return _connection.Query<TransactionDto>(
                    _transactionGetByAccountIdProcedure,
                    new { AccountId = id },
                    commandType: CommandType.StoredProcedure
                ).ToList();
        }

        public List<TransactionDto> GetByAccountIds(List<int> accountIds)
        {
            return _connection.Query<TransactionDto>(
                    _transactionGetByAccountIdsProcedure,
                    new { @Ids = accountIds },
                    commandType: CommandType.StoredProcedure
                ).ToList();
        }
    }
}
