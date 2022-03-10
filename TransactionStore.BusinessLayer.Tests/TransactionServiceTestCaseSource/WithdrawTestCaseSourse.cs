using Marvelous.Contracts;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TransactionServiceTestCaseSource
{
    public class WithdrawTestCaseSourse : IEnumerable
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

            var accountTransactions = new List<TransactionDto>()
            {
                new TransactionDto
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 500,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                },
                new TransactionDto
                {
                    Id = 2,
                    AccountId = 1,
                    Amount = -200,
                    Currency = Currency.RUB,
                    Type = TransactionType.Withdraw
                }

            };

            int id = 1;

            yield return new object[] { withdraw, accountTransactions, id };
        }
    }
}
