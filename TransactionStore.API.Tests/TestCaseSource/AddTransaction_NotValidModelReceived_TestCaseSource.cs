using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using System;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class AddTransaction_NotValidModelReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var currency = -1;
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var transactionRequestModel = new TransactionRequestModel
            {
                AccountId = -1,
                Amount = -1,
                Currency = (Currency)currency,
            };
            var transactionId = 1;

            yield return new object[] { identityResponseModel, transactionRequestModel, transactionId };
        }
    }
}
