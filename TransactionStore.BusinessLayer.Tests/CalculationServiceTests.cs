using Marvelous.Contracts.Enums;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests : VerifyLoggerHelper<CalculationService>
    {
        private CalculationService _calculationService;
        private IMemoryCache _cache;

        [SetUp]
        public void Setup()
        {
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
            _logger = new Mock<ILogger<CalculationService>>();
            _calculationService = new CalculationService(currencyRatesService, _logger.Object);
        }

        [TestCase(Currency.RUB, Currency.EUR, 0.92)]
        [TestCase(Currency.EUR, Currency.CNY, 700)]
        [TestCase(Currency.USD, Currency.EUR, 91)]
        [TestCase(Currency.USD, Currency.CNY, 637)]
        [TestCase(Currency.EUR, Currency.USD, 109.89)]
        [TestCase(Currency.CNY, Currency.USD, 15.7)]
        public void ConvertCurrency_ValidRequestReceived_ShouldConvertCurrency(Currency currencyFrom, Currency currencyTo, decimal expected)
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
        public void ConvertCurrency_CurrencyNotCorrect_ShouldThrowCurrencyNotReceivedException(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            //given          
            var expectedMessage = "The request for the currency value was not received";

            //when
            CurrencyNotReceivedException? exception = Assert.Throws<CurrencyNotReceivedException>(() =>
            _calculationService.ConvertCurrency(currencyFrom, currencyTo, expected));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Exception: The request for the currency value was not received", LogLevel.Error);
        }
    }
}
