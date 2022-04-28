using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class WithdrawDateDoesntMatchTestcaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var transactionModel = new TransactionModel()
            {
                Id = 1,
                Amount = 100,
                AccountId = 1,
                Currency = Currency.RUB
            };
            decimal balance = 1000m;
            DateTime dateTime = DateTime.Today;

            yield return new object[] { transactionModel, balance, dateTime };
        }
    }
}
