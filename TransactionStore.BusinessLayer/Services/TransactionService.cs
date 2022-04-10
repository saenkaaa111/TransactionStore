using AutoMapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using System.Collections;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;
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
            ICalculationService calculationService, IBalanceRepository balanceRepository, IMapper mapper, ILogger<TransactionService> logger)
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
            CheckCurrencyHelper.CheckCurrency(transactionModel.Currency);
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;

            return await _transactionRepository.AddTransaction(transaction);
        }

        public async Task<List<long>> AddTransfer(TransferModel transferModel)
        {
            _logger.LogInformation("Request to add Transfer");
            CheckCurrencyHelper.CheckCurrency(transferModel.CurrencyFrom);
            CheckCurrencyHelper.CheckCurrency(transferModel.CurrencyTo);
            var convertResult = _calculationService.ConvertCurrency(transferModel.CurrencyFrom,
                transferModel.CurrencyTo, transferModel.Amount);

            var transfer = _mapper.Map<TransferDto>(transferModel);
            transfer.ConvertedAmount = convertResult;

            var accountBalance = await _balanceRepository.GetBalanceByAccountId(transferModel.AccountIdFrom);

            if (transfer.Amount < accountBalance)
            {
                return await _transactionRepository.AddTransfer(transfer);
            }
            else
            {
                _logger.LogError("Exception: Insufficient funds");
                throw new InsufficientFundsException("Insufficient funds");
            }
        }

        public async Task<long> Withdraw(TransactionModel transactionModel)
        {
            _logger.LogInformation("Request to add Withdraw");
            CheckCurrencyHelper.CheckCurrency(transactionModel.Currency);
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);
            var accountBalance = await _balanceRepository.GetBalanceByAccountId(transactionModel.AccountId);

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

        public async Task<ArrayList> GetTransactionsByAccountIds(List<int> ids)
        {
            _logger.LogInformation($"Request to add transaction by AccountId = {ids}");
            var listTransactionAll = await _transactionRepository.GetTransactionsByAccountIds(ids);
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
            _logger.LogInformation($"Request to add transaction by id = {id}");

            var transaction = await _transactionRepository.GetTransactionById(id);

            return _mapper.Map<TransactionModel>(transaction);
        }
    }
}
