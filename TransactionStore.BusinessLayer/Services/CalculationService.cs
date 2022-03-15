using Marvelous.Contracts;
using NLog;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ICurrencyRates _currencyRates;
        private readonly ITransactionRepository _transactionRepository;
        private static Logger _logger;

        public CalculationService(ICurrencyRates currencyRates, ITransactionRepository transactionRepository)
        {
            _currencyRates = currencyRates;
            _logger = LogManager.GetCurrentClassLogger();
            _transactionRepository = transactionRepository;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.Debug($"Запрос на конвертацию валюты с {currencyFrom} в {currencyTo} ");

            var rates = _currencyRates.Rates;
            rates.TryGetValue(currencyFrom, out var currencyFromValue);
            rates.TryGetValue(currencyTo, out var currencyToValue);

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 4);

            _logger.Debug("Валюта конвертирована");
            return convertAmount;
        }

        public decimal GetAccountBalance(int accauntId)
        {
            _logger.Debug("Запрос на получение всех транзакция у текущего аккаунта");
            var transaction = _transactionRepository.GetByAccountId(accauntId);
            _logger.Debug("Транзакции получены");

            if (transaction == null)
                throw new NullReferenceException("Аккаунта не найдено");
            decimal balance = 0;
            foreach (var item in transaction)
            {
                balance += ConvertCurrency(item.Currency, Currency.USD, item.Amount);
            }
            _logger.Debug("Баланс посчитан");
            return balance;
        }
    }
}
