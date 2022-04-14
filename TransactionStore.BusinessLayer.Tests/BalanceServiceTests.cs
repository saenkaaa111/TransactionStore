using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class BalanceServiceTests 
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private CalculationService _calculationService;
        private Mock<ILogger<BalanceService>> _loggerMock;
        private BalanceService _balanceService;
        private IMemoryCache _cache;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _loggerMock = new Mock<ILogger<BalanceService>>();
            _cache = new MemoryCache(new MemoryCacheOptions());

            var currencyRatesService = new CurrencyRatesService(_cache);
            currencyRatesService.CurrencyRates = new()
            {
                { "USDRUB", 99.00m },
                { "USDEUR", 0.91m },
                { "USDJPY", 121.64m },
                { "USDCNY", 6.37m },
                { "USDTRY", 14.82m },
                { "USDRSD", 106.83m }
            };
            _calculationService = new CalculationService(currencyRatesService, new Mock<ILogger<CalculationService>>().Object);
            _balanceService = new BalanceService(_transactionRepositoryMock.Object, _calculationService, _loggerMock.Object);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrency_NoTransactionsFound_ShouldReturnZero()
        {
            //given
            var expected = 0;
            var ids = new List<int> { 77 };
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(new List<TransactionDto>());

            //when
            var actual = _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR).Result;

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(b => b.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify("Request to receive all transactions from the current account", LogLevel.Information);
            LoggerVerify("Transactions received", LogLevel.Information);
            LoggerVerify("Balance calculated", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrencyTestCaseSourseTestCaseSource))]
        public void GetBalanceByAccountIdsInGivenCurrency_FewTransactionsFound_ShoulCalculateBalance(
            decimal expected, List<int> ids, List<TransactionDto> transactions)
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR).Result;

            //then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(t => t.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify("Request to receive all transactions from the current account", LogLevel.Information);
            LoggerVerify("Transactions received", LogLevel.Information);
            LoggerVerify("Balance calculated", LogLevel.Information);
        }

        private void LoggerVerify(string message, LogLevel logLevel)
        {
            _loggerMock.Verify(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
