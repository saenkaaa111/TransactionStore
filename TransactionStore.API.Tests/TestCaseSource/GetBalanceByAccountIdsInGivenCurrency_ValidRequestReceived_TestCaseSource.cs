using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            IdentityResponseModel identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var ids = new List<int> { 1 };

            decimal balance = 777m;

            yield return new object[] { identityResponseModel, ids, balance } ;
        }


    }
}
