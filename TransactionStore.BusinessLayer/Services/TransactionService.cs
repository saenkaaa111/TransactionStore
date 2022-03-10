using AutoMapper;
using Marvelous.Contracts;
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

            return _transactionRepository.AddTransaction(transaction);
        }

        public List<int> AddTransfer(TransferModel transactionModel)
        {
            var transactionFrom = new TransactionDto()
            {
                AccountId = transactionModel.AccountIdFrom,
                Currency = transactionModel.CurrencyFrom,
                Amount = transactionModel.Amount * -1,
                Type = TransactionType.Transfer
            };            
            DateTime dateTransactionFrom = _transactionRepository.AddTransferFrom(transactionFrom);
            
            //TODO transfer to another currency
            var transactionTo = new TransactionDto()
            {
                Amount = transactionModel.Amount,
                AccountId = transactionModel.AccountIdTo,
                Currency = transactionModel.CurrencyTo,
                Type = TransactionType.Transfer,
                Date = dateTransactionFrom
            };            
            var idTransactionTo = _transactionRepository.AddTransferTo(transactionTo);

            return new List<int>() { 4, idTransactionTo }; //переделать id
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

        public List<TransactionModel> GetTransactionsByAccountIds(List<int> accountIds)
        {
            var transactions = _transactionRepository.GetTransactionsByAccountIds(accountIds);

            return _mapper.Map<List<TransactionModel>>(transactions);
        }
    }
}
