using AutoMapper;
using Marvelous.Contracts;
using NLog;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICalculationService _calculationService;
        private readonly IMapper _mapper;
        private static Logger _logger;

        public TransactionService(ITransactionRepository transactionRepository) 
        {
            _transactionRepository = transactionRepository;
        }
        public TransactionService(ITransactionRepository transactionRepository, 
            ICalculationService calculationService, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _calculationService = calculationService;
            _mapper = mapper;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public int AddDeposit(TransactionModel transactionModel)
        {
            _logger.Debug("Запрос на добавление Deposit");
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;

            return _transactionRepository.AddTransaction(transaction);
        }

        public List<int> AddTransfer(TransferModel transactionModel)
        {
            _logger.Debug("Запрос на добавление Transfer");

            var convertResult = _calculationService.ConvertCurrency(transactionModel.CurrencyFrom, 
                transactionModel.CurrencyTo, transactionModel.Amount);

            var transferDto = _mapper.Map<TransferDto>(transactionModel);
            transferDto.ConvertedAmount = convertResult;
            return _transactionRepository.AddTransfer(transferDto);
        }

        public int Withdraw(TransactionModel transactionModel)
        {
            _logger.Debug("Запрос на добавление Withdraw");
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);
            var accountTransactions = GetByAccountId(transactionModel.AccountId);
            var accountBalance = accountTransactions.Select(t => t.Amount).Sum();

            if (withdraw.Amount < accountBalance)
            {
                withdraw.Amount = transactionModel.Amount *= -1;
                withdraw.Type = TransactionType.Withdraw;

                return _transactionRepository.AddTransaction(withdraw);
            }
            else
            {
                _logger.Debug("Exception: Недостаточно средств на счете");
                throw new InsufficientFundsException("Недостаточно средств на счете");
            }
        }

        public List<TransactionModel> GetByAccountId(int id)
        {
            _logger.Debug($"Запрос на получение транзакциий по AccountId = {id}") ;

            var transactions = _transactionRepository.GetByAccountId(id);
            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public List<TransactionModel> GetTransactionsByAccountIds(List<int> accountIds)
        {
            _logger.Debug($"Запрос на получение транзакциий по AccountIds ");
            
            var transactions = _transactionRepository.GetTransactionsByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public TransactionModel GetTransactionById(int id)
        {
            _logger.Debug($"Запрос на получение транзакциий по id = {id}");

            var transaction = _transactionRepository.GetTransactionById(id);
            
            return _mapper.Map<TransactionModel>(transaction);
        }
        
        public decimal GetBalanceByAccountId(int accountId)
        {
            _logger.Debug($"Запрос на получение баланса по accountId = {accountId}");

            var balance = _transactionRepository.GetAccountBalance(accountId);
            return balance;

        }
        public decimal GetBalanceByAccountIds(List<int> accountId)
        {
            var balance = _calculationService.GetAccountBalance(accountId);
            return balance;

        }
    }
}
