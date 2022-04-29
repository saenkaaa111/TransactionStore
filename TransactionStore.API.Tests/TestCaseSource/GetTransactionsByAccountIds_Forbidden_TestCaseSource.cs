using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;
using System.Collections.Generic;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetTransactionsByAccountIds_Forbidden_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            var ids = new List<int> { 1, 2 };

            yield return new object[] { identityResponseModel, ids };
        }
    }
}
