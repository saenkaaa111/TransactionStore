using Marvelous.Contracts;
using Microsoft.Extensions.Logging;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ICurrencyRates _currencyRates;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<CalculationService> _logger;

        public CalculationService(ICurrencyRates currencyRates, ITransactionRepository transactionRepository,
            ILogger<CalculationService> logger)
        {
            _currencyRates = currencyRates;
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.LogInformation($"Запрос на конвертацию валюты с {currencyFrom} в {currencyTo} ");

            var rates = _currencyRates.Rates;
            rates.TryGetValue(currencyFrom, out var currencyFromValue);
            rates.TryGetValue(currencyTo, out var currencyToValue);

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 4);

            _logger.LogInformation("Валюта конвертирована");
            return convertAmount;
        }

        public decimal GetAccountBalance(int accauntId)
        {
            _logger.LogInformation("Запрос на получение всех транзакция у текущего аккаунта");
            var transaction = _transactionRepository.GetByAccountId(accauntId);
            _logger.LogInformation("Транзакции получены");

            if (transaction == null)
            {
                _logger.LogError("Error: Аккаунта не найдено");
                throw new NullReferenceException("Аккаунта не найдено");
            }

            decimal balance = 0;
            foreach (var item in transaction)
            {
                balance += ConvertCurrency(item.Currency, Currency.USD, item.Amount);
            }

            _logger.LogInformation("Баланс посчитан");
            return balance;
        }
    }
}
