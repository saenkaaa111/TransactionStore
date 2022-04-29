using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;
using TransactionStore.BusinessLayer.Models;

namespace TransactionStore.API.Tests.TestCaseSource
{
    internal class AddTransfer_ValidRequestReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var transferRequestModel = new TransferRequestModel
            {
                Amount = 100m,
                AccountIdFrom = 1,
                AccountIdTo = 2,
                CurrencyFrom = Currency.RUB,
                CurrencyTo = Currency.EUR
            };
            var transactionModelFirst = new TransactionModel
            {
                Id = 1,
                AccountId = 1,
                Amount = -100m,
                Currency = Currency.RUB,
                Type = TransactionType.Transfer
            };
            var transactionModelSecond = new TransactionModel
            {
                Id = 2,
                AccountId = 2,
                Amount = 1.2m,
                Currency = Currency.EUR,
                Type = TransactionType.Transfer
            };
            var transferIds = new List<long> { 1, 2 };

            yield return new object[] { identityResponseModel, transferRequestModel, 
                transactionModelFirst, transactionModelSecond, transferIds };
        }
    }
}
