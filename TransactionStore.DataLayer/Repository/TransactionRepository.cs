using Dapper;
using System.Data;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        private const string _transactionAddTransferFromProcedure = "dbo.Transaction_InsertTransferFrom";
        private const string _transactionAddTransferToProcedure = "dbo.Transaction_InsertTransferTo";
        private const string _transactionGetByAccountIdProcedure = "dbo.Transaction_SelectByAccountId";
        private const string _transactionGetByAccountIdsProcedure = "dbo.Transaction_SelectByAccountIds";

        public TransactionRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public int AddTransaction(TransactionDto transaction)
        {
            using IDbConnection connection = Connection;

            return connection.QueryFirstOrDefault<int>(
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
        }
        public DateTime AddTransferFrom(TransactionDto transaction)
        {
            using IDbConnection connection = Connection;

            return connection.QueryFirstOrDefault<DateTime>(
                _transactionAddTransferFromProcedure,
                new
                {
                    transaction.Amount,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public int AddTransferTo(TransactionDto transaction)
        {
            using IDbConnection connection = Connection;

            return connection.QueryFirstOrDefault<int>(
                _transactionAddTransferToProcedure,
                new
                {
                    transaction.Date,
                    transaction.Amount,
                    transaction.AccountId,
                    transaction.Type,
                    transaction.Currency
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<TransactionDto> GetByAccountId(int id)
        {
            using IDbConnection connection = Connection;

            return connection.Query<TransactionDto>(
                _transactionGetByAccountIdProcedure,
                new { AccountId = id },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public List<TransactionDto> GetTransactionsByAccountIds(List<int> accountIds)
        {
            using IDbConnection connection = Connection;

            var tvpTable = new DataTable();
            tvpTable.Columns.Add(new DataColumn("AccountId", typeof(int)));
            accountIds.ForEach(id => tvpTable.Rows.Add(id));

            return connection.Query<TransactionDto>(
                    _transactionGetByAccountIdsProcedure,
                    new { tvp = tvpTable.AsTableValuedParameter("[dbo].[AccountTVP]") },
                    commandType: CommandType.StoredProcedure
               ).ToList();
        }
    }
}
