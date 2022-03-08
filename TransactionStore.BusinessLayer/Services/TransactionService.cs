using AutoMapper;
using CurrencyEnum;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public int AddDeposit(TransactionModel transactionModel)
        {
            var transaction = _mapper.Map<TransactionDto>(transactionModel);

            transaction.Type = TransactionType.Deposit;
            transaction.Date = DateTime.Now;

            return _transactionRepository.AddTransaction(transaction);
        }

        public List<int> AddTransfer(TransactionModel transactionModel, int accountIdTo, int currencyTo)
        {
            var transaction = _mapper.Map<TransactionDto>(transactionModel);
            transaction.Amount *= -1;
            transaction.Date = DateTime.Now;
            transaction.Type = TransactionType.Transfer;
            var idTransactionFrom = _transactionRepository.AddTransaction(transaction);
            //TODO transfer to another currency
            transaction.Amount *= -1;
            transaction.AccountId = accountIdTo;
            transaction.Currency = (Currency)currencyTo;
            var idTransactionTo = _transactionRepository.AddTransaction(transaction);

            return new List<int>() { idTransactionFrom, idTransactionTo };
        }

        public int Withdraw(TransactionModel transactionModel)
        {
            var withdraw = _mapper.Map<TransactionDto>(transactionModel);
            var accountTransactions = GetByAccountId(transactionModel.AccountId);
            var accountBalance = accountTransactions.Select(t => t.Amount).Sum();

            if (withdraw.Amount < accountBalance)
            {
                withdraw.Amount = transactionModel.Amount *= -1;
                withdraw.Type = TransactionType.Withdraw;
                withdraw.Date = DateTime.Now;

                return _transactionRepository.AddTransaction(withdraw);
            }
            else
            {
                throw new InsufficientFundsException("Недостаточно средств на счете");
            }
        }

        public List<TransactionModel> GetByAccountId(int id)
        {
            var transactions = _transactionRepository.GetByAccountId(id);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }

        public List<TransactionModel> GetByAccountIds(List<int> accountIds)
        {
            var transactions = _transactionRepository.GetByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }
    }
}
