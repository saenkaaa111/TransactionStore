using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.ResponseModels;
using Microsoft.Extensions.Configuration;

namespace TransactionStore.BusinessLayer.Helpers
{
    public class InitializationHelper : IInitializationHelper
    {
        private readonly IRequestHelper _requestHelper;
        private readonly IConfiguration _configuration;
        public InitializationHelper(IRequestHelper requestHelper, IConfiguration configuration)
        {
            _requestHelper = requestHelper;
            _configuration = configuration;
        }

        public async Task InitializeConfigs()
        {
            var token = await _requestHelper.SendRequestForConfigs<string>("https://piter-education.ru:6042",
                AuthEndpoints.ApiAuth + AuthEndpoints.TokenForMicroservice);

            var configs = await _requestHelper
                .SendRequestForConfigs<IEnumerable<ConfigResponseModel>>("https://piter-education.ru:6040",
                ConfigsEndpoints.Configs, token!.Data);

            foreach (var c in configs!.Data)
            {
                _configuration[c.Key] = c.Value;
            }
        }
    }
}
