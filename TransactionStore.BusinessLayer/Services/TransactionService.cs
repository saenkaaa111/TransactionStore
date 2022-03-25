using AutoMapper;
using Marvelous.Contracts.Enums;
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
            var accountBalance = await GetBalanceByAccountId(transactionModel.AccountId);

            if (withdraw.Amount < accountBalance)
            {
                withdraw.Amount = transactionModel.Amount *= -1;
                withdraw.Type = TransactionType.Withdraw;

                return await _transactionRepository.AddTransaction(withdraw);
            }
            else
            {
                _logger.LogError("Exception: Insufficient funds");
                throw new InsufficientFundsException("Insufficient funds");
            }
        }

        public async Task<ArrayList> GetTransactionsByAccountId(long id)
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

        public async Task<List<TransactionModel>> GetTransactionsByAccountIds(List<long> accountIds)
        {
            _logger.LogInformation($"Request to add transaction by AccountIds ");

            var transactions = await _transactionRepository.GetTransactionsByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public async Task<TransactionModel> GetTransactionById(long id)
        {
            _logger.LogInformation($"ЗRequest to add transaction by id = {id}");

            var transaction = await _transactionRepository.GetTransactionById(id);

            return _mapper.Map<TransactionModel>(transaction);
        }

        public async Task<decimal> GetBalanceByAccountId(long accountId)
        {
            _logger.LogInformation($"Request to add balance by AccountId = {accountId}");

            var balance = await _transactionRepository.GetAccountBalance(accountId);
            return balance;

        }

        public async Task<decimal> GetBalanceByAccountIds(List<long> accountId)
        {
            _logger.LogInformation($"Request to add balance by AccountIds");

            var balance = await _calculationService.GetAccountBalance(accountId);
            return balance;
        }

        public bool CheckCurrency(Currency currency)
        {
            _logger.LogInformation($"Request to check currency");
            if (currency == Currency.RUB || currency == Currency.USD ||
                currency == Currency.EUR || currency == Currency.JPY ||
                currency == Currency.CNY || currency == Currency.RSD ||
                currency == Currency.TRY)
                return true;
            else
                throw new Exception("The request for the currency value was not received");
        }
    }
}
