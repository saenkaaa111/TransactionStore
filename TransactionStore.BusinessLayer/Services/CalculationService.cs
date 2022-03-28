using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private ICurrencyRatesService _currencyRatesService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<CalculationService> _logger;
        public const Currency BaseCurrency = Currency.USD;

        public CalculationService(ITransactionRepository transactionRepository, ICurrencyRatesService currencyRates,
            ILogger<CalculationService> logger)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _currencyRatesService = currencyRates;
        }

        public decimal ConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal amount)
        {
            _logger.LogInformation($"Request to convert currency from {currencyFrom} to {currencyTo}");
            var rates = _currencyRatesService.Pairs;

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
                throw new CurrencyNotReceivedException("The request for the currency value was not received");

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 2);

            _logger.LogInformation("Curency converted");

            return convertAmount;
        }

        public async Task<decimal> GetAccountBalance(List<int> accountId)
        {
            _logger.LogInformation("Request to receive all transactions from the current account");
            var listTransactions = new List<TransactionDto>();

            foreach (var item in accountId)
            {
                var listTransactionsFromOneAccount = new List<TransactionDto>();
                listTransactionsFromOneAccount = await _transactionRepository.GetTransactionsByAccountIdMinimal(item);

                foreach (var transaction in listTransactionsFromOneAccount)
                {
                    listTransactions.Add(transaction);
                }
            }

            _logger.LogInformation("Transactions received");

            if (listTransactions.Count == 0)
                throw new NullReferenceException("No transactions found");

            decimal balance = 0;

            foreach (var item in listTransactions)
            {
                balance += ConvertCurrency(item.Currency, BaseCurrency, item.Amount);
            }

            _logger.LogInformation("Balance calculated");

            return balance;
        }
    }
}
