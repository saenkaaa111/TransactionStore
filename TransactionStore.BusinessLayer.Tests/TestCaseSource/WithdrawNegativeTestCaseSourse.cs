using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class WithdrawNegativeTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var withdraw = new TransactionModel()
            {
                Id = 1,
                Amount = 100,
                AccountId = 1,
                Currency = Currency.RUB
            };

            decimal balance = 0m;
            DateTime dateTime = DateTime.Now;
            
            yield return new object[] { withdraw, balance, dateTime};
        }
    }
}
