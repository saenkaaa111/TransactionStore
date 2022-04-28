using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class TransferTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var transferModel = new TransferModel()
            {
                Amount = 100,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };

            var transferDto = new TransferDto()
            {
                Amount = 100,
                ConvertedAmount = 1.2m,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };

            var expected = new List<long>() { 1, 2 };

            decimal balance = 1000m;
            DateTime dateTime = DateTime.Today;
            decimal convertedAmount = 1.2m;

            yield return new object[] { transferModel, transferDto, expected, balance, convertedAmount, dateTime };
        }
    }
}
