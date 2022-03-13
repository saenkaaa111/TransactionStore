﻿using Marvelous.Contracts;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.BusinessLayer.Tests
{
    public class CalculationServiceTests
    {
        private CalculationService _service;

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
            currencyRates.Setup(x => x.Rates).Returns(_rates);
            _service = new CalculationService(currencyRates.Object);
        }

        [TestCase(Currency.RUB, Currency.EUR, 0.7759)]
        [TestCase(Currency.GBP, Currency.CNY, 900)]
        public void ConvertCurrencyTest(Currency currencyFrom, Currency currencyTo, decimal expected)
        {
            var actual = _service.ConvertCurrency(currencyFrom, currencyTo, 100.0m);

            Assert.AreEqual(expected, actual);
        }


    }
}