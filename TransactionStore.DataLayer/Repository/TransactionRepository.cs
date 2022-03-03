using Dapper;
using System.Data;
using System.Data.SqlClient;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private const string _connectionString = "Data Source = 80.78.240.16; Database=Transaction;User Id = student; Password=qwe!23;";


        public int AddTransaction(TransactionA transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            string procName = "dbo.Transaction_Insert";
            return connection.QueryFirstOrDefault<int>(
                    procName,
                    new
                    {
                        Amount = transaction.Amount,
                        Date = transaction.Date,
                        AccountId = transaction.Account,
                        Type = transaction.Type
                    },
                    commandType: CommandType.StoredProcedure
                );
        }
    }
}
