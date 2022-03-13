using Dapper;
using NLog;
using System.Data;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.DataLayer.Repository
{
    public class TransactionRepository : BaseRepository, ITransactionRepository
    {
        private const string _transactionAddProcedure = "dbo.Transaction_Insert";
        private const string _transactionGetByAccountIdProcedure = "dbo.Transaction_SelectByAccountId";
        private const string _transactionGetByAccountIdsProcedure = "dbo.Transaction_SelectByAccountIds";
        private const string _transactionGetByIdProcedure = "dbo.Transaction_SelectById";
        private const string _transactionTransfer = "dbo.Transaction_Transfer";
        private static Logger _logger;

        public TransactionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public int AddTransaction(TransactionDto transaction)
        {
            _logger.Debug("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.Debug("Подключение произведено.");

            var id = connection.QueryFirstOrDefault<int>(
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

            _logger.Debug($"Транзакция типа {transaction.Type} с id = {id} добавлена в БД.");
            return id;
        }

        public List<int> AddTransfer(TransferDto transaction)
        {
            _logger.Debug("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.Debug("Подключение произведено.");

            var listId = (IDictionary<string, object>)connection.QueryFirstOrDefault<dynamic>(
            _transactionTransfer,
                new
                {
                    transaction.Amount,
                    transaction.ConvertedAmount,
                    transaction.AccountIdFrom,
                    transaction.AccountIdTo,
                    transaction.CurrencyFrom,
                    transaction.CurrencyTo
                },
                commandType: CommandType.StoredProcedure
                );
            _logger.Debug($"Транзакция типа Transfer с id = {(int)listId.Values.First()}, {(int)listId.Values.Last()} добавлены в БД.");

            return new List<int>() { (int)listId.Values.First(), (int)listId.Values.Last() };

        }

        public List<TransactionDto> GetByAccountId(int id)
        {
            _logger.Debug("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.Debug("Подключение произведено.");

            var listTransactions = connection.Query<TransactionDto>(
                _transactionGetByAccountIdProcedure,
                new { AccountId = id },
                commandType: CommandType.StoredProcedure
            ).ToList();

            _logger.Debug($"Транзакция по AccountId = {id} получены.");

            return listTransactions;
        }

        public List<TransactionDto> GetTransactionsByAccountIds(List<int> accountIds)
        {
            _logger.Debug("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.Debug("Подключение произведено.");

            var tvpTable = new DataTable();
            tvpTable.Columns.Add(new DataColumn("AccountId", typeof(int)));
            accountIds.ForEach(id => tvpTable.Rows.Add(id));

            var listTransactions = connection.Query<TransactionDto>(
                    _transactionGetByAccountIdsProcedure,
                    new { tvp = tvpTable.AsTableValuedParameter("[dbo].[AccountTVP]") },
                    commandType: CommandType.StoredProcedure
               ).ToList();

            _logger.Debug($"Транзакция по AccountIds = {accountIds} получены.");
            return listTransactions;
        }

        public TransactionDto GetTransactionById(int id)
        {
            _logger.Debug("Подключение к базе данных.");
            using IDbConnection connection = Connection;
            _logger.Debug("Подключение произведено.");

            var listTransactions = connection.QuerySingle<TransactionDto>(
                _transactionGetByIdProcedure, new { Id = id },
                commandType: CommandType.StoredProcedure);

            _logger.Debug($"Транзакция по Id = {id} получены.");

            return listTransactions;

        }
    }
}