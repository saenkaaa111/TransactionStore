
using AutoMapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Data.SqlClient;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Exceptions;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ICalculationService _calculationService;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository,
            ICalculationService calculationService, IBalanceRepository balanceRepository, IMapper mapper,
            ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _balanceRepository = balanceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<long> AddDeposit(TransactionModel transactionModel)
        {
            _logger.LogInformation("Request to add Deposit");
            CheckCurrencyHelper checkCurrency = new CheckCurrencyHelper();
            checkCurrency.CheckCurrency(transactionModel.Currency);
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;

            return await _transactionRepository.AddDeposit(transaction);
        }

        public async Task<List<long>> AddTransfer(TransferModel transferModel)
        {
            _logger.LogInformation("Request to add Transfer");
            CheckCurrencyHelper checkCurrency = new CheckCurrencyHelper();
            checkCurrency.CheckCurrency(transferModel.CurrencyFrom);
            checkCurrency.CheckCurrency(transferModel.CurrencyTo);
            var convertResult = _calculationService.ConvertCurrency(transferModel.CurrencyFrom,
                transferModel.CurrencyTo, transferModel.Amount);

            var transfer = _mapper.Map<TransferDto>(transferModel);
            transfer.ConvertedAmount = convertResult;

            var lastTransactionDate = await CheckBalanceAndGetLastTransactionDate(transferModel.AccountIdFrom, transfer.Amount);

            try
            {
                return await _transactionRepository.AddTransfer(transfer, lastTransactionDate);
            }
            catch (SqlException ex)
            {
                _logger.LogError("Error: Flood crossing");
                throw new DbTimeoutException(ex.Message);
            }                               
                
        }

        public async Task<long> Withdraw(TransactionModel transactionModel)
        {
            _logger.LogInformation("Request to add Withdraw");
            CheckCurrencyHelper checkCurrency = new CheckCurrencyHelper();
            checkCurrency.CheckCurrency(transactionModel.Currency);
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);

            var lastTransactionDate = await CheckBalanceAndGetLastTransactionDate(withdraw.AccountId, withdraw.Amount);

            withdraw.Amount = transactionModel.Amount *= -1;
            
            try
            {
                return await _transactionRepository.AddTransaction(withdraw, lastTransactionDate);
            }
            catch (TransactionsConflictException ex)
            {
                _logger.LogError("Error: Flood crossing");
                throw new DbTimeoutException(ex.Message);
            }         
        }

        public async Task<ArrayList> GetTransactionsByAccountIds(List<int> ids)
        {
            _logger.LogInformation($"Request to get transactions by AccountId = {ids}");
            var listTransactionAll = await _transactionRepository.GetTransactionsByAccountIds(ids);

            if (listTransactionAll is null)
            {
                _logger.LogError($"Error: Transactions weren't found");
                throw new TransactionNotFoundException($"Transactions weren't found");
            }
            var listTransactionSort = listTransactionAll.GroupBy(x => x.Id).Select(x => x.First());

            var transactionsWithoutTransfer = listTransactionAll.Where(x => x.Type != TransactionType.Transfer);

            var resultList = new ArrayList();

            foreach (var item in transactionsWithoutTransfer)
            {
                resultList.Add(item);
            }

            var transactionsOnlyTransfer = listTransactionSort.Where(x => x.Type == TransactionType.Transfer).ToList();

            for (int i = 0; i < transactionsOnlyTransfer.Count(); i = i + 2)
            {
                TransferDto transfer = new()
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

        public async Task<TransactionModel> GetTransactionById(long id)
        {
            _logger.LogInformation($"Request to get transaction by id = {id}");
            var transaction = await _transactionRepository.GetTransactionById(id);

            if (transaction is not null)
            {
                return _mapper.Map<TransactionModel>(transaction);
            }
            else
            {
                _logger.LogError($"Error: Transaction with Id = {id} wasn't found");
                throw new TransactionNotFoundException($"Transaction with Id = {id} wasn't found");
            }
        }

        public async Task<DateTime> CheckBalanceAndGetLastTransactionDate(int accountId, decimal amount)
        {
            var (accountBalance, lastTransactionDate) = await _balanceRepository.GetBalanceByAccountId(accountId);

            if (accountBalance < amount)
            {
                _logger.LogError("Exception: Flood crossing");
                throw new DbTimeoutException("Flood crossing");
                return false;
            }
            if ((decimal)accountBalanceAndDate.Result[0] < amount)
            {
                _logger.LogError("Error: Insufficient funds");
                throw new InsufficientFundsException("Insufficient funds");

            }
            _logger.LogInformation("Correct information");
            return lastTransactionDate;

        }
    }
}