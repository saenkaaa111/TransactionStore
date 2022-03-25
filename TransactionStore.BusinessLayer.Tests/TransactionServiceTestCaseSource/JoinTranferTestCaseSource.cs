using Marvelous.Contracts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TransactionServiceTestCaseSource
{
    public class JoinTranferTestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var dateTime = DateTime.Now;
            var dateTime1 = DateTime.Now;
            var dateTime2 = DateTime.Now;

            var transactions = new List<TransactionDto>
            {
                new TransactionDto
                {
                    AccountId = 1,
                    Currency = Currency.RUB,
                    Date = dateTime,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    AccountId = 2,
                    Currency = Currency.EUR,
                    Date = dateTime,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    AccountId = 3,
                    Currency = Currency.RUB,
                    Date = dateTime1,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    AccountId = 4,
                    Currency = Currency.EUR,
                    Date = dateTime1,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    AccountId = 5,
                    Currency = Currency.RUB,
                    Date = dateTime2,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    AccountId = 6,
                    Currency = Currency.EUR,
                    Date = dateTime2,
                    Type = TransactionType.Transfer,
                },
                new TransactionDto
                {
                    Type = TransactionType.Deposit,
                },
                new TransactionDto
                {
                    Type = TransactionType.Withdraw,
                },
                new TransactionDto
                {
                    Type = TransactionType.ServicePayment,
                },
            };

            yield return new object[] { transactions };
        }
    }
}
