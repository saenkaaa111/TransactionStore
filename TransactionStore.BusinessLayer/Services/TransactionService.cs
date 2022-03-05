﻿using AutoMapper;
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

            return _transactionRepository.AddDeposit(transaction);
        }
    }
}
