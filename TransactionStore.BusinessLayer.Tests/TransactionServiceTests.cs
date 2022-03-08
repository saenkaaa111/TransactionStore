using AutoMapper;
using Moq;
using NUnit.Framework;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Services.Interfaces;
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
        
        [TestCase( 4)]
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

        [TestCase(77)]
        public void WithdrawTest(int expected)
        {
            // given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
            var transactionModel = new TransactionModel() { Type = TransactionType.Withdraw, Amount = 777, AccountId = 7 };

            // when
            int actual = _service.Withdraw(transactionModel);

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);

            Assert.AreEqual(expected, actual);
        }
    }
}