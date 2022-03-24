//using AutoMapper;
//using Marvelous.Contracts;
//using Microsoft.Extensions.Logging;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using TransactionStore.BuisnessLayer.Configuration;
//using TransactionStore.BusinessLayer.Services;
//using TransactionStore.DataLayer.Entities;
//using TransactionStore.DataLayer.Repository;

//namespace TransactionStore.BusinessLayer.Tests
//{
//    public class TransactionServiceTests
//    {

//        private Mock<ITransactionRepository> _transactionRepositoryMock;
//        private TransactionService _transactionService;
//        private Mock<ICalculationService> _calculationService;
//        private IMapper _mapper;
//        private Mock<ILogger<TransactionService>> _logger;

//        [SetUp]
//        public void Setup()
//        {
//            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
//            _transactionRepositoryMock = new Mock<ITransactionRepository>();
//            _logger = new Mock<ILogger<TransactionService>>();
//            _calculationService = new Mock<ICalculationService>();
//            _transactionService = new TransactionService(_transactionRepositoryMock.Object,
//                _calculationService.Object, _mapper, _logger.Object);
//        }

//        [TestCase(4)]
//        [TestCase(896)]
//        public void AddDepositTest(int expected)
//        {
//            // given
//            _transactionRepositoryMock.Setup(d => d.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
//            var deposit = new TransactionModel() { Type = TransactionType.Deposit, Amount = 600, AccountId = 6 };

//            // when
//            int actual = _transactionService.AddDeposit(deposit);

//            // then
//            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
//            Assert.AreEqual(expected, actual);
//        }

//        [Test]
//        public void AddTransferTest()
//        {
//            // given
//            var expected = new List<int>() { 1, 2 };
//            _transactionRepositoryMock.Setup(d => d.AddTransfer(It.IsAny<TransferDto>())).Returns(expected);

//            var transfer = new TransferModel()
//            {
//                Amount = 100,
//                AccountIdFrom = 1,
//                AccountIdTo = 2,
//                CurrencyFrom = Currency.RUB,
//                CurrencyTo = Currency.EUR
//            };

//            // when
//            var actual = _transactionService.AddTransfer(transfer);

//            // then
//            _transactionRepositoryMock.Verify(s => s.AddTransfer(It.IsAny<TransferDto>()), Times.Once);
//            Assert.AreEqual(expected, actual);
//        }

//        [TestCaseSource(typeof(WithdrawTestCaseSourse))]
//        public void WithdrawTest(TransactionModel transactionModel, List<TransactionDto> accountTransactions, int expected)
//        {
//            // given
//            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>())).Returns(expected);
//            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(transactionModel.AccountId))
//                .Returns(accountTransactions);

//            // when
//            int actual = _transactionService.Withdraw(transactionModel);

//            // then
//            _transactionRepositoryMock.Verify(s => s.AddTransaction(It.IsAny<TransactionDto>()), Times.Once);
//            Assert.AreEqual(expected, actual);
//        }

//        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
//        public void WithdrawNegativeTest_ShouldThrowInsufficientFundsException(TransactionModel transactionModel,
//            List<TransactionDto> accountTransactions)
//        {
//            // given
//            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
//            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(transactionModel.AccountId))
//                .Returns(accountTransactions);
//            var expectedMessage = "Недостаточно средств на счете";

//            // when
//            InsufficientFundsException? exception = Assert.Throws<InsufficientFundsException>(() =>
//            _transactionService.Withdraw(transactionModel));

//            // then
//            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
//        }

//        [TestCaseSource(typeof(GetTransactionsByAccountIdsTestCaseSourse))]
//        public void GetTransactionsByAccountIdsTest(List<int> ids, List<TransactionDto> transactions,
//            List<TransactionModel> expected)
//        {
//            //given
//            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).Returns(transactions);

//            //when
//            var actual = _transactionService.GetTransactionsByAccountIds(ids);

//            //then
//            Assert.AreEqual(actual, expected);
//            _transactionRepositoryMock.Verify(s => s.GetTransactionsByAccountIds(ids), Times.Once);
//        }

//        [Test]
//        public void GetTransactionByIdTest()
//        {
//            //given
//            _transactionRepositoryMock.Setup(w => w.GetTransactionById(It.IsAny<int>())).Returns(It.IsAny<TransactionDto>());

//            //when
//            _transactionService.GetTransactionById(It.IsAny<int>());

//            //then
//            _transactionRepositoryMock.Verify(s => s.GetTransactionById(It.IsAny<int>()), Times.Once);
//        }

//        [Test]
//        public void JoinTransferTransactionsTest()
//        {
//            var dateTime = DateTime.Now;
//            var dateTime1 = DateTime.Now;
//            var dateTime2 = DateTime.Now;

//            //given
//            var transactions = new List<TransactionDto>
//            {
//                new TransactionDto
//                {
//                    AccountId = 1,
//                    Currency = Currency.RUB,
//                    Date = dateTime,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    AccountId = 2,
//                    Currency = Currency.EUR,
//                    Date = dateTime,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    AccountId = 3,
//                    Currency = Currency.RUB,
//                    Date = dateTime1,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    AccountId = 4,
//                    Currency = Currency.EUR,
//                    Date = dateTime1,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    AccountId = 5,
//                    Currency = Currency.RUB,
//                    Date = dateTime2,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    AccountId = 6,
//                    Currency = Currency.EUR,
//                    Date = dateTime2,
//                    Type = TransactionType.Transfer,
//                },
//                new TransactionDto
//                {
//                    Type = TransactionType.Deposit,
//                },
//                new TransactionDto
//                {
//                    Type = TransactionType.Withdraw,
//                },
//                new TransactionDto
//                {
//                    Type = TransactionType.Service,
//                },
//            };

//            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountId(It.IsAny<int>())).Returns(transactions);

//            //when
//            var result = _transactionService.GetTransactionsByAccountId(It.IsAny<int>());
//            var expected = transactions.Count(x => x.Type == TransactionType.Transfer) / 2;
//            var actual = result.Count(x => x.Type == TransactionType.Transfer);

//            //then
//            Assert.AreEqual(expected, actual);
//        }
//    }
//}