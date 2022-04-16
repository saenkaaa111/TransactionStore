using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetBalanceByAccountIdsInGivenCurrency_Forbidden_TestCaseSource : IEnumerable 
    {
        public IEnumerator GetEnumerator()
        {
            var expectedMessage = "MarvelousFrontendResource doesn't have access to this endpiont";
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousFrontendResource.ToString()
            };

            yield return new object[] { expectedMessage, identityResponseModel };
        }
    }
}
