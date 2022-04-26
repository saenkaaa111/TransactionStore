using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using System.Threading.Tasks;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CurrencyRatesServiceTests
    {
        private CurrencyRatesService _currencyRatesService;
        private IMemoryCache _cache;

        [SetUp]
        public void Setup()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _currencyRatesService = new CurrencyRatesService(_cache)
            {
                CurrencyRates = new()
                {
                    { "USDRUB", 99.00m },
                    { "USDEUR", 0.91m },
                    { "USDJPY", 121.64m },
                    { "USDCNY", 6.37m },
                    { "USDTRY", 14.82m },
                    { "USDRSD", 106.83m }
                },
                BaseCurrency = Currency.USD

            };
        }

        [Test]
        public async Task SaveCurrencyRates_ValidRequestReceived_ShouldSaveNewCurrencyRates()
        {
            //given
            CurrencyRatesExchangeModel currencyModel = new CurrencyRatesExchangeModel()
            {
                Rates = new()
                {
                    { "USDRUB", 90.00m },
                    { "USDEUR", 0.9m },
                    { "USDJPY", 120.00m },
                    { "USDCNY", 7.00m },
                    { "USDTRY", 15.00m },
                    { "USDRSD", 107.00m }
                },
                BaseCurrency = Currency.RUB
            };

            //when
            _currencyRatesService.SaveCurrencyRates(currencyModel);

            //then
            Assert.AreEqual(_currencyRatesService.CurrencyRates, currencyModel.Rates);
            Assert.AreEqual(_currencyRatesService.BaseCurrency, Currency.USD);
        }

        [Test]
        public async Task SaveBaseCurrency_ValidRequestReceived_ShouldSaveNewBaseCurrency()
        {
            //given
            CurrencyRatesExchangeModel currencyModel = new CurrencyRatesExchangeModel()
            {
                Rates = new()
                {
                    { "USDRUB", 90.00m },
                    { "USDEUR", 0.9m },
                    { "USDJPY", 120.00m },
                    { "USDCNY", 7.00m },
                    { "USDTRY", 15.00m },
                    { "USDRSD", 107.00m }
                },
                BaseCurrency = Currency.RUB
            };

            //when
            _currencyRatesService.SaveBaseCurrency(currencyModel);

            //then
            Assert.AreNotEqual(_currencyRatesService.CurrencyRates, currencyModel.Rates);
            Assert.AreEqual(_currencyRatesService.BaseCurrency, Currency.RUB);
        }

        [Test]
        public async Task GetCurrencyRates_ValidRequestReceived_ShouldGetCurrencyRates()
        {
            //given            
            //when
            var actual = _currencyRatesService.GetCurrencyRates();

            //then
            Assert.AreEqual(actual, _currencyRatesService.CurrencyRates);
        }

        [Test]
        public async Task GetCurrencyRates_CurrencyNull_ShouldGetCurrencyRatesFromCache()
        {
            //given
            _currencyRatesService = new CurrencyRatesService(_cache)
            {
                CurrencyRates = null,
                BaseCurrency = Currency.USD

            };
            //when
            var actual = _currencyRatesService.GetCurrencyRates();

            //then
            Assert.IsNotNull(actual);
        }
    }
}
