using AutoMapper;
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
        
        public List<int> AddTransfer(TransactionModel transactionModel, int accountIdTo)
        {
            var transaction = _mapper.Map<TransactionDto>(transactionModel);
            transaction.Amount *= -1;
            transaction.Date = DateTime.Now;
            transaction.Type=TransactionType.Transfer;            
            var idTransactionFrom = _transactionRepository.AddTransaction(transaction);
            //TODO transfer to another currency
            transaction.Amount *= -1;
            transaction.AccountId = accountIdTo;
            var idTransactionTo = _transactionRepository.AddTransaction(transaction);

            return new List<int>() { idTransactionFrom, idTransactionTo };
        }
    }
}
