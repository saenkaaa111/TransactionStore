﻿using AutoMapper;
using Marvelous.Contracts;
using Microsoft.Extensions.Logging;
using System.Collections;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICalculationService _calculationService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository,
            ICalculationService calculationService, IMapper mapper, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<long> AddDeposit(TransactionModel transactionModel)
        {
            _logger.LogInformation("Запрос на добавление Deposit");
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;

            return await _transactionRepository.AddTransaction(transaction);
        }

        public async Task<List<long>> AddTransfer(TransferModel transactionModel)
        {
            _logger.LogInformation("Запрос на добавление Transfer");

            var convertResult = await _calculationService.ConvertCurrency(transactionModel.CurrencyFrom,
                transactionModel.CurrencyTo, transactionModel.Amount);

            var transferDto = _mapper.Map<TransferDto>(transactionModel);
            transferDto.ConvertedAmount = convertResult;
            return await _transactionRepository.AddTransfer(transferDto);
        }

        public async Task<long> Withdraw(TransactionModel transactionModel)
        {
            _logger.LogInformation("Запрос на добавление Withdraw");
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);
            var accountBalance = await GetBalanceByAccountId(transactionModel.AccountId);
            
            if (withdraw.Amount < accountBalance)
            {
                withdraw.Amount = transactionModel.Amount *= -1;
                withdraw.Type = TransactionType.Withdraw;

                return await _transactionRepository.AddTransaction(withdraw);
            }
            else
            {
                _logger.LogError("Exception: Недостаточно средств на счете");
                throw new InsufficientFundsException("Недостаточно средств на счете");
            }
        }

        public async Task<ArrayList> GetTransactionsByAccountId(long id)
        {
            _logger.LogInformation($"Запрос на получение транзакциий по AccountId = {id}");
            var transactions = await _transactionRepository.GetTransactionsByAccountId(id);
            var listTransaction = _mapper.Map<List<TransactionModel>>(transactions);
            var transactionsWithoutTransfer = listTransaction.Where(x => x.Type != TransactionType.Transfer);
            var resultList = new ArrayList();
            foreach (var item in transactionsWithoutTransfer)
            {
                resultList.Add(item);
            }

            var transactionsOnlyTransfer = listTransaction.Where(x => x.Type == TransactionType.Transfer).ToList();

            for (int i = 0; i < transactionsOnlyTransfer.Count(); i = i + 2)
            {
                TransferDto transfer = new TransferDto()
                {
                    IdFrom = transactionsOnlyTransfer[i].Id,
                    IdTo = transactionsOnlyTransfer[i + 1].Id,
                    AccountIdFrom = transactionsOnlyTransfer[i].AccountId,
                    AccountIdTo = transactionsOnlyTransfer[i + 1].AccountId,
                    Amount = transactionsOnlyTransfer[i].Amount,
                    ConvertedAmount = transactionsOnlyTransfer[i + 1].Amount,
                    CurrencyFrom = transactionsOnlyTransfer[i].Currency,
                    CurrencyTo = transactionsOnlyTransfer[i + 1].Currency,
                    Date = transactionsOnlyTransfer[i].Date,
                    Type = transactionsOnlyTransfer[i].Type,

                };
                resultList.Add(transfer);
            }

            return resultList;
        }

        public async Task<List<TransactionModel>> GetTransactionsByAccountIds(List<long> accountIds)
        {
            _logger.LogInformation($"Запрос на получение транзакциий по AccountIds ");

            var transactions = await _transactionRepository.GetTransactionsByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public async Task<TransactionModel> GetTransactionById(long id)
        {
            _logger.LogInformation($"Запрос на получение транзакциий по id = {id}");

            var transaction = await _transactionRepository.GetTransactionById(id);

            return _mapper.Map<TransactionModel>(transaction);
        }

        public async Task<decimal> GetBalanceByAccountId(long accountId)
        {
            _logger.LogInformation($"Запрос на получение баланса по accountId = {accountId}");

            var balance = await _transactionRepository.GetAccountBalance(accountId);
            return balance;

        }

        public async Task<decimal> GetBalanceByAccountIds(List<long> accountId)
        {
            _logger.LogInformation($"Запрос на получение баланса по accountIds");
            
            var balance = await _calculationService.GetAccountBalance(accountId);
            return balance;
        }


    }
}
