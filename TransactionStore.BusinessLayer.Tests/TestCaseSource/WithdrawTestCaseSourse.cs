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
            var transactionModel = new TransactionModel()
            {
                Amount = 100,
                AccountId = 1,
                Currency = Currency.RUB
            };
            
            var transactionDto = new TransactionDto()
            {
                Amount = -100,
                AccountId = 1,
                Currency = Currency.RUB
            };

            long expected = 1;
            decimal balance = 300m;
            DateTime dateTime = DateTime.Today;
            
            yield return new object[] { transactionModel, transactionDto, expected, balance, dateTime };
        }
    }
}
