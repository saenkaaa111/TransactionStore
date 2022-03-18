using Marvelous.Contracts;
using NLog;
using TransactionStore.DataLayer.Entities;
using TransactionStore.BusinessLayer.Services;
﻿using Microsoft.Extensions.Logging;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrencyRates _currencyRates;
        private readonly ILogger<CalculationService> _logger;
        public const string BaseCurrency = "USD";

        public CalculationService(ITransactionRepository transactionRepository, ICurrencyRates currencyRates,
            ILogger<CalculationService> logger)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _currencyRates = currencyRates;
        }

        public decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount)
        {
            _logger.LogInformation($"Запрос на конвертацию валюты с {currencyFrom} в {currencyTo} ");
            var rates = _currencyRates.GetRates();

            rates.TryGetValue($"{BaseCurrency}{currencyFrom}", out var currencyFromValue);
            rates.TryGetValue($"{BaseCurrency}{currencyTo}", out var currencyToValue);

            if (currencyFrom == BaseCurrency)
                currencyFromValue = 1m;

            if (currencyTo == BaseCurrency)
                currencyToValue = 1m;

            var convertAmount = decimal.Round(currencyToValue / currencyFromValue * amount, 4);

            _logger.LogInformation("Валюта конвертирована");
            return convertAmount;
        }

        public decimal GetAccountBalance(List<int> accauntId)
        {
            _logger.LogInformation("Запрос на получение всех транзакция у текущего аккаунта");
            var listTransactions = new List<TransactionDto> ();
            var listTransactionsFromOneAccount = new List<TransactionDto> ();
            foreach (var item in accauntId)
            {
                listTransactionsFromOneAccount = _transactionRepository.GetByAccountId(item);
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
                balance += ConvertCurrency(item.Currency.ToString(), BaseCurrency, item.Amount);
                // поставил ToString пока
            }

            _logger.LogInformation("Баланс посчитан");
            return balance;
        }
    }
}
