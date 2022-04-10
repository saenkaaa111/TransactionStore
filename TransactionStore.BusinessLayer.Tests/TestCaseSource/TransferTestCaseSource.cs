using Marvelous.Contracts.Enums;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class TransferTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var transfer = new TransferModel()
            {
                Amount = 100,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };

            var expected = new List<long>() { 1, 2 };

            decimal balance = 1000m;

            yield return new object[] { transfer, expected, balance };
        }
    }
}
