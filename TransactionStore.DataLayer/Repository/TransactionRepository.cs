using Dapper;
using System.Data;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";

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
    }
}
