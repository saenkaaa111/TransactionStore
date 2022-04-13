using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests
    {
        private CalculationService _calculationService;
        private Mock<ILogger<CalculationService>> _logger;
        

        [SetUp]
        public void Setup()
        {
            var currencyRatesService = new CurrencyRatesService();
            currencyRatesService.CurrencyRates = new()
            {
                { "USDRUB", 99.00m },
                { "USDEUR", 0.91m },
                { "USDJPY", 121.64m },
                { "USDCNY", 6.37m },
                { "USDTRY", 14.82m },
                { "USDRSD", 106.83m }

            };
            _logger = new Mock<ILogger<CalculationService>>();
            _calculationService = new CalculationService(currencyRatesService, _logger.Object);
            
        }

        [TestCase(Currency.RUB, Currency.EUR, 0.92)]
        [TestCase(Currency.EUR, Currency.CNY, 700)]
        [TestCase(Currency.USD, Currency.EUR, 91)]
        [TestCase(Currency.USD, Currency.CNY, 637)]
        [TestCase(Currency.EUR, Currency.USD, 109.89)]
        [TestCase(Currency.CNY, Currency.USD, 15.7)]
        public void ConvertCurrency_ValidRequestRecieved_ShouldConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            //given            
            //when
            var actual = _calculationService.ConvertCurrency(currencyFrom, currencyTo, 100.0m);

            //then
            Assert.AreEqual(expected, actual);
            LoggerVerify("Currency converted", LogLevel.Information);
        }
        

        [TestCase(Currency.ARS, Currency.EUR, 91)]
        [TestCase(Currency.FJD, Currency.CNY, 637)]
        [TestCase(Currency.RUB, Currency.DOP, 91)]
        [TestCase(Currency.RUB, Currency.KRW, 637)]
        public void ConvertCurrency_CurrencyNotCorrect_(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            //given          
            var expectedMessage = "The request for the currency value was not received";

            //when
            CurrencyNotReceivedException? exception = Assert.Throws<CurrencyNotReceivedException>(() =>
            _calculationService.ConvertCurrency(currencyFrom,currencyTo, expected));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: The request for the currency value was not received", LogLevel.Error);
        
        }

        private void LoggerVerify(string message, LogLevel logLevel)
        {
            _logger.Verify(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
