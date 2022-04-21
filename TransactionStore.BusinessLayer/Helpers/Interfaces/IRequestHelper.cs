using Marvelous.Contracts.ResponseModels;

namespace TransactionStore.BusinessLayer.Helpers
{
    public interface IRequestHelper
    {
        Task<IdentityResponseModel> SendRequestCheckValidateToken(string url, string path, string jwtToken);
    }
}