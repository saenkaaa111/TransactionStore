using Marvelous.Contracts.Enums;
using NUnit.Framework;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CheckCurrencyHelperTests
    {
        private CheckCurrencyHelper _checkCurensyService;


        [SetUp]
        public void Setup()
        {
            _checkCurensyService = new CheckCurrencyHelper();
        }

        [TestCase(Currency.RUB)]
        [TestCase(Currency.EUR)]
        [TestCase(Currency.JPY)]
        [TestCase(Currency.CNY)]
        [TestCase(Currency.TRY)]
        [TestCase(Currency.RSD)]
        public void CheckCurrency_ValidRequestReceived_ShouldReturnTrue(Currency currency)
        {
            //given            
            //when
            var actual = _checkCurensyService.CheckCurrency(currency);

            //then
            Assert.IsTrue(actual);

        }

        [TestCase(Currency.SAR)]
        [TestCase(Currency.TVD)]
        [TestCase(Currency.KPW)]
        public void CheckCurrency_CurrencyNotIncludedInListAllowed_CurrencyNotReceivedException(Currency currency)
        {
            //given
            var expectedMessage = "The request for the currency value was not received";

            //when
            CurrencyNotReceivedException? exception = Assert.Throws<CurrencyNotReceivedException>(() =>
            _checkCurensyService.CheckCurrency(currency));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));

        }
    }
}
