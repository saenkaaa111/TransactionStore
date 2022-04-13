using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class BalanceServiceTests
    {
        private Mock<ITransactionRepository> _transactionRepository;
        private CalculationService _calculationService;
        private Mock<ILogger<TransactionService>> _logger;
        private BalanceService _balanceService;

        [SetUp]
        public void Setup()
        {
            _transactionRepository = new Mock<ITransactionRepository>();
            _logger = new Mock<ILogger<TransactionService>>();
            var currencyRatesService = new CurrencyRatesService();
            currencyRatesService.CurrencyRates = new()
            {


            };

            _calculationService = new CalculationService( currencyRatesService, null, (new Mock<ILogger<CalculationService>>()).Object);
            _balanceService = new BalanceService(_transactionRepository.Object,
                _calculationService, _logger.Object);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrencyTest_NoTransactionsFound_ShouldReturnZero()
        {
            //given
            var expected = 0;
            var ids = new List<int> { 77 };
            _transactionRepository.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(new List<TransactionDto>());

            //when
            var actual = _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR).Result;

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepository.Verify(b => b.GetTransactionsByAccountIds(ids), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                    string.Equals("Request to receive all transactions from the current account", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Transactions received", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Balance calculated", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrencyTestCaseSourseTestCaseSource))]
        public async Task GetBalanceByAccountIdsInGivenCurrencyTest_SeveralTransactionsFound_ShoulCalculateBalance(
            decimal expected, List<int> ids, List<TransactionDto> transactions)
        {
            //given
            _transactionRepository.Setup(t => t.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);
            //_calculationService.Setup(c => c.ConvertCurrency(Currency.RUB, Currency.EUR, 100m)).Returns(expected);

            //when
            var actual = await _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR);

            //then
            Assert.AreEqual(expected, actual);
            _transactionRepository.Verify(t => t.GetTransactionsByAccountIds(ids), Times.Once);
            //_calculationService.Verify(t => t.ConvertCurrency(Currency.RUB, Currency.EUR, 1000m), Times.Exactly(4));
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                    string.Equals("Request to receive all transactions from the current account", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Transactions received", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Balance calculated", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
