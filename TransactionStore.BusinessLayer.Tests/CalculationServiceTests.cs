using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests
    {
        private CalculationService _calculationService;
        private Mock<ICurrencyRatesService> _currencyRatesServiceMock;
        private Mock<ILogger<CalculationService>> _loggerMock;
        private IMemoryCache _cacheMock;

        [SetUp]
        public void Setup()
        {
            _currencyRatesServiceMock = new Mock<ICurrencyRatesService>();
            _cacheMock = new MemoryCache(new MemoryCacheOptions());
            _loggerMock = new Mock<ILogger<CalculationService>>();
            _calculationService = new CalculationService(_currencyRatesServiceMock.Object, _cacheMock,
                _loggerMock.Object);
        }

        [TestCase(Currency.RUB, Currency.EUR, 0.7759)]
        [TestCase(Currency.GBP, Currency.CNY, 900)]
        public void ConvertCurrencyTest(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            //given
            _currencyRatesServiceMock.Setup(c => c.SaveCurrencyRates(
                It.IsAny<CurrencyRatesExchangeModel>()));

            //when
            var actual = _calculationService.ConvertCurrency(currencyFrom, currencyTo, 100.0m);

            //then
            Assert.AreEqual(expected, actual);
        }
    }
}
