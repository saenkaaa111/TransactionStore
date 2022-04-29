using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Tests.TestCaseSource;
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class BalanceServiceTests : VerifyLoggerHelper<BalanceService>
    {
        private Mock<ITransactionRepository> _transactionRepositoryMock;
        private CalculationService _calculationService;
        private BalanceService _balanceService;
        private IMemoryCache _cache;

        [SetUp]
        public void Setup()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = new Mock<ILogger<BalanceService>>();
            var currencyRatesService = new CurrencyRatesService(_cache)
            {
                CurrencyRates = new()
                {
                    { "USDRUB", 99.00m },
                    { "USDEUR", 0.91m },
                    { "USDJPY", 121.64m },
                    { "USDCNY", 6.37m },
                    { "USDTRY", 14.82m },
                    { "USDRSD", 106.83m }
                }
            };
            _calculationService = new CalculationService(currencyRatesService, new Mock<ILogger<CalculationService>>().Object);
            _balanceService = new BalanceService(_transactionRepositoryMock.Object, _calculationService, _logger.Object);
        }
         
        [Test]
        public async Task GetBalanceByAccountIdsInGivenCurrency_NoTransactionsFound_ShouldReturnZero()
        {
            //given
            var expected = 0;
            var ids = new List<int> { 77 };
            _transactionRepositoryMock.Setup(w => w.GetTransactionsByAccountIds(ids)).ReturnsAsync(new List<TransactionDto>());

            //when
            var actual = await _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR);

            //then
            Assert.AreEqual(actual, expected);
            _transactionRepositoryMock.Verify(b => b.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify("Request to receive all transactions from the current account", LogLevel.Information);
            LoggerVerify("Transactions received", LogLevel.Information);
            LoggerVerify("Balance calculated", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrencyTestCaseSourseTestCaseSource))]
        public async Task GetBalanceByAccountIdsInGivenCurrency_FewTransactionsFound_ShoulCalculateBalance(
            decimal expected, List<int> ids, List<TransactionDto> transactions)
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = await _balanceService.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.EUR);

            //then
            Assert.AreEqual(expected, actual);
            _transactionRepositoryMock.Verify(t => t.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify("Request to receive all transactions from the current account", LogLevel.Information);
            LoggerVerify("Transactions received", LogLevel.Information);
            LoggerVerify("Balance calculated", LogLevel.Information);
        }
    }
}
