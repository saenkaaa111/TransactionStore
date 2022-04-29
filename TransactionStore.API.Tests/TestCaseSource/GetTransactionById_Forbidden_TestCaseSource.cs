using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using System.Collections;

namespace TransactionStore.API.Tests.TestCaseSource
{
    public class GetTransactionById_Forbidden_TestCaseSource : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            long id = 1;

            yield return new object[] { identityResponseModel, id };
        }
    }
}
