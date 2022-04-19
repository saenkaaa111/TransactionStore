using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TestCaseSource
{
    public class WithdrawTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var withdraw = new TransactionModel()
            {
                Amount = 100,
                AccountId = 1,
                Currency = Currency.RUB
            };
            
            var withdrawDto = new TransactionDto()
            {
                Amount = -100,
                AccountId = 1,
                Currency = Currency.RUB
            };

            long id = 1;
            decimal balance = 300m;
            DateTime dateTime = DateTime.Now;
            ArrayList array = new ArrayList() { balance, dateTime };
            
            yield return new object[] { withdraw, withdrawDto, id, array  };
        }
    }
}
