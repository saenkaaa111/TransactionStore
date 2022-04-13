using AutoMapper;
using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BuisnessLayer.Configuration;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class TransactionServiceTests
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private TransactionService _transactionService;
        private Mock<ICalculationService> _calculationServiceMock;
        private Mock<IBalanceRepository> _balanceRepositoryMock;
        private IMapper _mapper;
        private Mock<ILogger<TransactionService>> _logger;

        [SetUp]
        public void Setup()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DataMapper>()));
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _balanceRepositoryMock = new Mock<IBalanceRepository>();
            _logger = new Mock<ILogger<TransactionService>>();
            _calculationServiceMock = new Mock<ICalculationService>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object,
            _calculationServiceMock.Object, _balanceRepositoryMock.Object, _mapper, _logger.Object);
        }


        [TestCaseSource(typeof(DepositTestCaseSourse))]
        public void AddDepositTest_ShouldAddDeposit(TransactionModel depositModel, TransactionDto depositDto, long expected)
        {
            //given
            _transactionRepositoryMock.Setup(d => d.AddTransaction(depositDto)).ReturnsAsync(expected);
            
            //when
            var actual = _transactionService.AddDeposit(depositModel).Result;

            // then
            _transactionRepositoryMock.Verify(s => s.AddTransaction(depositDto), Times.Once);
            Assert.AreEqual(expected, actual);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Request to add Deposit", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

        }


        [TestCaseSource(typeof(TransferTestCaseSource))]
        public void AddTransfer_ValidRequestRecieved_ShouldAddTransfer(TransferModel transferModel, 
            TransferDto transferDto, List<long> expected, decimal balance, decimal convertedAmount)
        {
            //given
            _transactionRepositoryMock.Setup(t => t.AddTransfer(transferDto)).ReturnsAsync(expected);
            _balanceRepositoryMock.Setup(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom)).ReturnsAsync(balance);
            _calculationServiceMock.Setup(c => c.ConvertCurrency(transferModel.CurrencyFrom,
                transferModel.CurrencyTo, transferModel.Amount)).Returns(convertedAmount);

            // when
            var actual = _transactionService.AddTransfer(transferModel).Result;

            // then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(t => t.AddTransfer(transferDto), Times.Once);
            _balanceRepositoryMock.Verify(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom), Times.Once);
            _calculationServiceMock.Verify(c => c.ConvertCurrency(transferModel.CurrencyFrom, transferModel.CurrencyTo,
                transferModel.Amount), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Request to add Transfer", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [TestCaseSource(typeof(TransferNegativeTestCaseSource))]
        public void AddTransfer_BalanceLessThanAmount_ShouldThrowInsufficientFundsException(TransferModel transferModel,
            decimal balance)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransfer(It.IsAny<TransferDto>()));
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transferModel.AccountIdFrom))
                .ReturnsAsync(balance);
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.ThrowsAsync<InsufficientFundsException>(() =>
            _transactionService.AddTransfer(transferModel));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            _balanceRepositoryMock.Verify(b => b.GetBalanceByAccountId(transferModel.AccountIdFrom), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Exception: Insufficient funds", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [TestCaseSource(typeof(WithdrawTestCaseSourse))]
        public void Withdraw_ValidRequestRecieved_ShouldAddTransation(TransactionModel transactionModel, TransactionDto  transactionDto, long expected, decimal balance)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(transactionDto)).ReturnsAsync(expected);
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transactionModel.AccountId))
                .ReturnsAsync(balance);

            //when
            var actual = _transactionService.Withdraw(transactionModel).Result;

            // then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(s => s.AddTransaction(transactionDto), Times.Once);
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(transactionModel.AccountId), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Request to add Withdraw", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }


        [TestCaseSource(typeof(WithdrawNegativeTestCaseSourse))]
        public void Withdraw_BalanceLessThenAmount_InsufficientFundsException(TransactionModel transactionModel)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.AddTransaction(It.IsAny<TransactionDto>()));
            _balanceRepositoryMock.Setup(w => w.GetBalanceByAccountId(transactionModel.AccountId))
                .ReturnsAsync(0);
            var expectedMessage = "Insufficient funds";

            //when
            InsufficientFundsException? exception = Assert.ThrowsAsync<InsufficientFundsException>(() =>
            _transactionService.Withdraw(transactionModel));

            // then
            _balanceRepositoryMock.Verify(s => s.GetBalanceByAccountId(transactionModel.AccountId), Times.Once);
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Exception: Insufficient funds", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

        }


        [TestCaseSource(typeof(GetTransactionsByAccountIdsTestCaseSourse))]
        public void GetTransactionsByAccountIdsTest(List<int> ids, List<TransactionDto> transactions,
            ArrayList expected)
        {
            //given
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = _transactionService.GetTransactionsByAccountIds(ids).Result;
            
            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(s => s.GetTransactionsByAccountIds(ids), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals($"Request to add transaction by AccountId = {ids}", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

        }


        [TestCase(77)]
        public void GetTransactionById_ValidRequestRecieved_ShouldGetTransaction(long id)
        {
            //given
            var transaction = new TransactionDto() { Id = 77 };
            _transactionRepositoryMock.Setup(w => w.GetTransactionById(id)).ReturnsAsync(transaction);

            //when
            var actual = _transactionService.GetTransactionById(id).Result;

            //then
            Assert.AreEqual(actual.Id, id);
            _transactionRepositoryMock.Verify(s => s.GetTransactionById(id), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals($"Request to add transaction by id = {id}", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}