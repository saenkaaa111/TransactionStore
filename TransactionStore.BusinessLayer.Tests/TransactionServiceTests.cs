using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.BusinessLayer.Tests.TransactionServiceTestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class TransactionServiceTests
    {

        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private ITransactionService _service;
        private readonly IMapper _mapper;

        public TransactionServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
        }


        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _service = new TransactionService(_transactionRepositoryMock.Object, _mapper);
        }

        [TestCase(4)]
        [TestCase(896)]
        public void AddDepositTest(int expected)
        {
            // given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
            var deposit = new TransactionModel() { Type = TransactionType.Deposit, Amount = 600, AccountId = 6 };
            
            // when
            int actual = _service.AddDeposit(deposit);

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }
        
        [TestCase(4)]
        [TestCase(896)]
        public void AddTransferTest(int expected )
        {
            // given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
            var listExpected = new[] { expected, expected };
            var deposit = new TransactionModel() { Type = TransactionType.Deposit, Amount = 600, AccountId = 6 };

            // when
            var actual = _service.AddTransfer(deposit, expected, expected);

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Exactly(2));
            Assert.AreEqual(listExpected, actual);
        }

        [TestCaseSource(typeof(WithdrawTestCaseSourse))]
        public void WithdrawTest(TransactionModel transactionModel, List<TransactionDto> accountTransactions, int expected)
        {
            // given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
            _transactionRepositoryMock.Setup(w => w.GetByAccountId(transactionModel.AccountId))
                .Returns(accountTransactions);

            // when
            int actual = _service.Withdraw(transactionModel);

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
        public void WithdrawNegativeTest_ShouldThrowInsufficientFundsException(TransactionModel transactionModel,
            List<TransactionDto> accountTransactions, int expected)
        {
            // given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _transactionRepositoryMock.Setup(w => w.GetByAccountId(transactionModel.AccountId))
                .Returns(accountTransactions);
            var expectedMessage = "Недостаточно средств на счете";

            // when
            InsufficientFundsException? exception = Assert.Throws<InsufficientFundsException>(() =>
            _service.Withdraw(transactionModel));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
        }
    }
}