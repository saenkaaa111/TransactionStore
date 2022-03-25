using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private ICurrencyRatesService _ratesService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<CalculationService> _logger;
        public const Currency BaseCurrency = Currency.USD;

        public CalculationService(ITransactionRepository transactionRepository, ICurrencyRatesService currencyRates,
            ILogger<CalculationService> logger)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _ratesService = currencyRates;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.LogInformation($"Запрос на конвертацию валюты с {currencyFrom} в {currencyTo} ");
            var rates = _ratesService.Pairs;

            if (rates == null)
            {
                string jsonread = File.ReadAllText("dictionary.json");
                rates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonread);
            }
            else
            {
                string json = JsonConvert.SerializeObject(rates, Formatting.Indented);
                File.WriteAllText("dictionary.json", json);
            }
            rates.TryGetValue($"{BaseCurrency}{currencyFrom}", out var currencyFromValue);
            rates.TryGetValue($"{BaseCurrency}{currencyTo}", out var currencyToValue);

            if (currencyFrom == BaseCurrency)
                currencyFromValue = 1m;

            if (currencyTo == BaseCurrency)
                currencyToValue = 1m;
            if (currencyFromValue == 0 || currencyToValue == 0)
                throw new InsufficientFundsException("Значение валюты не было получено");

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 2);

            _logger.LogInformation("Валюта конвертирована");
            return convertAmount;
        }

        public async Task<decimal> GetAccountBalance(List<long> accauntId)
        {
            _logger.LogInformation("Запрос на получение всех транзакция у текущего аккаунта");
            var listTransactions = new List<TransactionDto>();
            var listTransactionsFromOneAccount = new List<TransactionDto>();
            foreach (var item in accauntId)
            {
                listTransactionsFromOneAccount = await _transactionRepository.GetTransactionsByAccountIdMinimal(item);
                foreach (var transaction in listTransactionsFromOneAccount)
                {
                    listTransactions.Add(transaction);
                }
            }
            _logger.LogInformation("Транзакции получены");

            if (listTransactions.Count == 0)
                throw new NullReferenceException("Транзакций не найдено");
            decimal balance = 0;
            foreach (var item in listTransactions)
            {
                balance += ConvertCurrency(item.Currency, BaseCurrency, item.Amount);
                // поставил ToString пока
            }

            _logger.LogInformation("Баланс посчитан");
            return balance;
        }
    }
}
