using Dapper;
using System.Data;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";

        public TransactionRepository(IDbConnection dbConnection) : base(dbConnection) { }

        public int AddDeposit(TransactionDto transaction)
        {
            return _connection.QueryFirstOrDefault<int>(
                    _transactionAddProcedure,
                    new
                    {
                        transaction.Amount,
                        Date = DateTime.Now,
                        transaction.AccountId,
                        Type = TransactionType.Deposit
                    },
                    commandType: CommandType.StoredProcedure
                );
        }
    }
}
