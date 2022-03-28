using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests
    {
        private CalculationService _calculationService;
        private ITransactionRepository _transactionRepository;
        private Mock<ILogger<CalculationService>> _logger;

        [SetUp]
        public void Setup()
        {
            var currencyRates = new Mock<ICurrencyRatesService>();
            _logger = new Mock<ILogger<CalculationService>>();
            _calculationService = new CalculationService(_transactionRepository, currencyRates.Object, _logger.Object);
            var rates = new Dictionary<string, decimal>
            {
                { "USDRUB", 105m },
                { "USDEUR", 0.9m },
                { "USDJPY", 118m },
                { "USDCNY", 6m },
                { "USDTRY", 14m },
                { "USDRSD", 106m }
            };
        }

        [TestCase(Currency.RUB, Currency.EUR, 0.7759)]
        [TestCase(Currency.GBP, Currency.CNY, 900)]
        public void ConvertCurrencyTest(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            var actual = _calculationService.ConvertCurrency(currencyFrom, currencyTo, 100.0m);

            Assert.AreEqual(expected, actual);
        }
    }
}
