using Marvelous.Contracts.Enums;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.BusinessLayer.Tests.TransactionServiceTestCaseSource
{
    public class GetTransactionsByAccountIdsTestCaseSourse : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var ids = new List<long>() { 1, 2 };

            var transactions = new List<TransactionDto>()
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
                    Currency = Currency.USD,
                    Type = TransactionType.Withdraw
                },
                new TransactionDto
                {
                    Id = 3,
                    AccountId = 2,
                    Amount = 700,
                    Currency = Currency.RUB,
                    Type = TransactionType.Transfer
                }
            };

            var expected = new List<TransactionModel>()
            {
                new TransactionModel
                {
                    Id = 1,
                    AccountId = 1,
                    Amount = 500,
                    Currency = Currency.RUB,
                    Type = TransactionType.Deposit
                },
                new TransactionModel
                {
                    Id = 2,
                    AccountId = 1,
                    Amount = -200,
                    Currency = Currency.USD,
                    Type = TransactionType.Withdraw
                },
                new TransactionModel
                {
                    Id = 3,
                    AccountId = 2,
                    Amount = 700,
                    Currency = Currency.RUB,
                    Type = TransactionType.Transfer
                }
            };

            yield return new object[] { ids, transactions, expected };
        }
    }
}
