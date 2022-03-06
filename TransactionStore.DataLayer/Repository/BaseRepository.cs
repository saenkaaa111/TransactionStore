using System.Data;

namespace TransactionStore.DataLayer.Repository
{
    public class BaseRepository
    {
        public IDbConnection _connection;

        public BaseRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }
    }
}
