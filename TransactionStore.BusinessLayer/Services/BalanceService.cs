using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository _balanceRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICalculationService _calculationService;
        private readonly ILogger<TransactionService> _logger;
        public const Currency BaseCurrency = Currency.USD;

        public BalanceService(IBalanceRepository balanceRepository, ITransactionRepository transactionRepository,
            ICalculationService calculationService, ILogger<TransactionService> logger)
        {
            _balanceRepository = balanceRepository;
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _logger = logger;
        }

        public async Task<decimal> GetBalanceByAccountIdsInGivenCurrency(List<int> accountIds, Currency currency)
        {
            _logger.LogInformation("Request to receive all transactions from the current account");
            var listTransactions = await _transactionRepository.GetTransactionsByAccountIdMinimal(accountIds);
            _logger.LogInformation("Transactions received");

            if (listTransactions.Count == 0)
                return 0m;

            decimal balance = 0;

            foreach (var item in listTransactions)
            {
                balance += _calculationService.ConvertCurrency(item.Currency, currency, item.Amount);
            }

            _logger.LogInformation("Balance calculated");

            return balance;
        }
    }
}
