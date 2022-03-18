using Marvelous.Contracts;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.BusinessLayer.Services.Interfaces;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests
    {
        private CalculationService _calculationService;
        private ITransactionRepository _transactionRepository;
        private Mock<ILogger<CalculationService>> _logger;

        private Dictionary<Currency, decimal> _rates = new()
        { 
            { Currency.USD, 1m },
            { Currency.RUB, 116m },
            { Currency.EUR, 0.9m },
            { Currency.CNY, 6.3m },
            { Currency.GBP, 0.7m },
        };

        [SetUp]
        public void Setup()
        {
            var currencyRates = new Mock<ICurrencyRates>();
            _logger = new Mock<ILogger<CalculationService>>();
            _calculationService = new CalculationService(currencyRates.Object, _transactionRepository, _logger.Object);
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
