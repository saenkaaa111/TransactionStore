using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class TransferNegativeTestCaseSource : IEnumerable
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
            decimal balance = 0m;
            DateTime dateTime = DateTime.Now;
            ArrayList array = new ArrayList() { balance, dateTime };

            yield return new object[] { transfer, array };
        }
    }
}
