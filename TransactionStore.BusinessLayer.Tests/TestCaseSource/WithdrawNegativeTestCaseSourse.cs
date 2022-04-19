﻿using Marvelous.Contracts.Enums;
using System;
using System.Collections;
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
            ArrayList array = new ArrayList() { balance, dateTime };

            yield return new object[] { withdraw, array };
        }
    }
}
