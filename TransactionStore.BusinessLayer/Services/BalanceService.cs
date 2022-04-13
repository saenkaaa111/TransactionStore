using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICalculationService _calculationService;
        private readonly ILogger<TransactionService> _logger;

        public BalanceService(ITransactionRepository transactionRepository, ICalculationService calculationService, 
            ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _logger = logger;
        }

        public async Task<decimal> GetBalanceByAccountIdsInGivenCurrency(List<int> accountIds, Currency currency)
        {
            _logger.LogInformation("Request to receive all transactions from the current account");
            var listTransactions = await _transactionRepository.GetTransactionsByAccountIds(accountIds);
            _logger.LogInformation("Transactions received");

            if (listTransactions.Count != 0)
            {
                var balance = 0m;

                foreach (var item in listTransactions)
                {
                    balance += _calculationService.ConvertCurrency(item.Currency, currency, item.Amount);
                }

                _logger.LogInformation("Balance calculated");

                return balance;
            }
            else
            {
                _logger.LogInformation("Balance calculated");

                return 0m;
            }
        }
    }
}
