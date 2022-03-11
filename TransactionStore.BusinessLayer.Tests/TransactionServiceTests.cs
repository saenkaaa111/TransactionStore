using AutoMapper;
using Marvelous.Contracts;
using Moq;
using NUnit.Framework;
using System;
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
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
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
            _transactionRepositoryMock.Setup(d => d.AddTransferFrom(It.IsAny<TransactionDto>())).Returns(new DateTime());
            _transactionRepositoryMock.Setup(d => d.AddTransferTo(It.IsAny<TransactionDto>())).Returns(expected);
            var listExpected = new[] { 4, expected };
            var transfer = new TransferModel() { Amount = 600, AccountIdFrom = 6, AccountIdTo = 7};

            // when
            var actual = _service.AddTransfer(transfer);

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransferFrom(It.IsAny<TransactionDto>()), Times.Once);
            _transactionRepositoryMock.Verify(s => s.AddTransferTo(It.IsAny<TransactionDto>()), Times.Once);
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
            List<TransactionDto> accountTransactions)
        {
            // given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _transactionRepositoryMock.Setup(w => w.GetByAccountId(transactionModel.AccountId))
                .Returns(accountTransactions);
            var expectedMessage = "������������ ������� �� �����";

            // when
            InsufficientFundsException? exception = Assert.Throws<InsufficientFundsException>(() =>
            _service.Withdraw(transactionModel));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(GetTransactionsByAccountIdsTestCaseSourse))]
        public void GetTransactionsByAccountIdsTest(List<int> ids, List<TransactionDto> transactions,
            List<TransactionModel> expected)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).Returns(transactions);

            //when
            var actual = _service.GetTransactionsByAccountIds(ids);

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(s => s.GetTransactionsByAccountIds(ids), Times.Once);
        }

        [Test]
        public void GetTransactionByIdTest()
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionById(It.IsAny<int>())).Returns(It.IsAny<TransactionDto>());

            //when
            _service.GetTransactionById(It.IsAny<int>());

            //then
            _transactionRepositoryMock.Verify(s => s.GetTransactionById(It.IsAny<int>()), Times.Once);
        }
    }
}