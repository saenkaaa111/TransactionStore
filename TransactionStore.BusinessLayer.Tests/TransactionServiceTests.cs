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
            _transactionRepositoryMock.Setup(d => d.AddDeposit(It.IsAny<TransactionDto>())).Returns(expected);
            var deposit = new TransactionModel() { Type = TransactionType.Deposit, Amount = 600, AccountId = 6 };
            // when
            int actual = _service.AddDeposit(new TransactionModel());

            // then
            _transactionRepositoryMock.Verify(s => s.AddDeposit(It.IsAny<TransactionDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }
    }
}