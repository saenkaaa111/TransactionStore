﻿using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using TransactionStore.DataLayer.Entities;
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
            var listTransactions = new List<TransactionDto>();

            foreach (var item in accountIds)
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
            if (currency == 0)
                currency = BaseCurrency;

            foreach (var item in listTransactions)
            {
                balance += _calculationService.ConvertCurrency(item.Currency, currency, item.Amount);
            }

            _logger.LogInformation("Balance calculated");

            return balance;
        }
    }
}