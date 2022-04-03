﻿using AutoMapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using System.Collections;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICalculationService _calculationService;
        private readonly IBalanceService _balanceService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;
        private readonly List<Currency> _currencyList = new() 
        { 
            Currency.RUB, 
            Currency.EUR, 
            Currency.USD, 
            Currency.JPY, 
            Currency.CNY, 
            Currency.RSD, 
            Currency.TRY
        };
        

        public TransactionService(ITransactionRepository transactionRepository, 
            ICalculationService calculationService, IBalanceService balanceService, IMapper mapper, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _balanceService = balanceService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<long> AddDeposit(TransactionModel transactionModel)
        {
            _logger.LogInformation("Request to add Deposit");
            CheckCurrency(transactionModel.Currency);
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;

            return await _transactionRepository.AddTransaction(transaction);
        }

        public async Task<List<long>> AddTransfer(TransferModel transactionModel)
        {
            _logger.LogInformation("Request to add Transfer");
            CheckCurrency(transactionModel.CurrencyFrom);
            CheckCurrency(transactionModel.CurrencyTo);
            var convertResult = _calculationService.ConvertCurrency(transactionModel.CurrencyFrom,
                transactionModel.CurrencyTo, transactionModel.Amount);

            var transferDto = _mapper.Map<TransferDto>(transactionModel);
            transferDto.ConvertedAmount = convertResult;
            return await _transactionRepository.AddTransfer(transferDto);
        }

        public async Task<long> Withdraw(TransactionModel transactionModel)
        {
            _logger.LogInformation("Request to add Withdraw");
            CheckCurrency(transactionModel.Currency);
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);
            var accountBalance = await _balanceService.GetBalanceByAccountIdsInGivenCurrency(new List<int>() { transactionModel.AccountId }, transactionModel.Currency);

            if (withdraw.Amount < accountBalance)
            {
                withdraw.Amount = transactionModel.Amount *= -1;
                
                return await _transactionRepository.AddTransaction(withdraw);
            }
            else
            {
                _logger.LogError("Exception: Insufficient funds");
                throw new InsufficientFundsException("Insufficient funds");
            }
        }

        public async Task<ArrayList> GetTransactionsByAccountId(int id)
        {
            _logger.LogInformation($"Request to add transaction by AccountId = {id}");
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

        public async Task<List<TransactionModel>> GetTransactionsByAccountIds(List<int> accountIds)
        {
            _logger.LogInformation($"Request to add transaction by AccountIds ");

            var transactions = await _transactionRepository.GetTransactionsByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public async Task<TransactionModel> GetTransactionById(long id)
        {
            _logger.LogInformation($"Request to add transaction by id = {id}");

            var transaction = await _transactionRepository.GetTransactionById(id);

            return _mapper.Map<TransactionModel>(transaction);
        }

        public bool CheckCurrency(Currency currency)
        {
            _logger.LogInformation($"Request to check currency");

            if (_currencyList.Contains(currency))
                return true; 

            else
                throw new CurrencyNotReceivedException("The request for the currency value was not received"); //переписать
            
        }
    }
}
