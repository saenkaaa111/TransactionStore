using AutoMapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TransactionServiceTestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class TransactionServiceTests
    {

        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private TransactionService _transactionService;
        private Mock<ICalculationService> _calculationService;
        private IMapper _mapper;
        private Mock<ILogger<TransactionService>> _logger;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _logger = new Mock<ILogger<TransactionService>>();
            _calculationService = new Mock<ICalculationService>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object,
                _calculationService.Object, _mapper, _logger.Object);
        }

        [TestCase(4)]
        [TestCase(896)]
        public void AddDepositTest(long expected)
        {
            //given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(It.IsAny<TransactionDto>())).ReturnsAsync(expected);
            var deposit = new TransactionModel() { Type = TransactionType.Deposit, Amount = 600, AccountId = 6, Currency = Currency.RUB };

            //when
            var actual = _transactionService.AddDeposit(deposit).Result;

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddTransferTest()
        {
            //given
            var expected = new List<long>() { 1, 2 };
            _transactionRepositoryMock.Setup(d => d.AddTransfer(It.IsAny<TransferDto>())).ReturnsAsync(expected);

            var transfer = new TransferModel()
            {
                Amount = 100,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };

            // when
            var actual = _transactionService.AddTransfer(transfer).Result;

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransfer(It.IsAny<TransferDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(WithdrawTestCaseSourse))]
        public void WithdrawTest(TransactionModel transactionModel, List<TransactionDto> accountTransactions, long expected, decimal balance)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>())).ReturnsAsync(expected);
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(transactionModel.AccountId))
                .ReturnsAsync(accountTransactions);
            _transactionRepositoryMock.Setup(w => w.GetAccountBalance(transactionModel.AccountId))
                .ReturnsAsync(balance);

            //when
            var actual = _transactionService.Withdraw(transactionModel).Result;

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
        public void WithdrawNegativeTest_ShouldThrowInsufficientFundsException(TransactionModel transactionModel,
            List<TransactionDto> accountTransactions)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(transactionModel.AccountId))
                .ReturnsAsync(accountTransactions);
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.ThrowsAsync<InsufficientFundsException>(() =>
            _transactionService.Withdraw(transactionModel));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
        }

        [TestCaseSource(typeof(GetTransactionsByAccountIdsTestCaseSourse))]
        public void GetTransactionsByAccountIdsTest(List<int> ids, List<TransactionDto> transactions,
            List<TransactionModel> expected)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = _transactionService.GetTransactionsByAccountIds(ids).Result;

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(s => s.GetTransactionsByAccountIds(ids), Times.Once);
        }

        [Test]
        public void GetTransactionByIdTest()
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionById(It.IsAny<long>())).ReturnsAsync(It.IsAny<TransactionDto>());

            //when
            _transactionService.GetTransactionById(It.IsAny<int>());

            //then
            _transactionRepositoryMock.Verify(s => s.GetTransactionById(It.IsAny<long>()), Times.Once);
        }

        [TestCaseSource(typeof(JoinTranferTestCaseSource))]
        public void JoinTransferTransactionsTest(List<TransactionDto> transactions)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(It.IsAny<int>())).ReturnsAsync(transactions);

            //when
            var result = _transactionService.GetTransactionsByAccountId(It.IsAny<int>()).Result;
            var expected = transactions.Count(x => x.Type == TransactionType.Transfer);
            var actual = result.Count;

            //then
            Assert.AreEqual(expected, actual);
        }
    }
}