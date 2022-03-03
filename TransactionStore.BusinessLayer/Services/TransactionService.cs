using AutoMapper;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services.Interfaces;

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

        public int AddTransaction(TransactionModel transactionModel)
        {
            var transaction = _mapper.Map<Transaction>(transactionModel);

            return _transactionRepository.AddTransaction(transaction);
        }
    }
}
