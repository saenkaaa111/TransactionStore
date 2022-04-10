using Marvelous.Contracts.Endpoints;
using Marvelous.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;

namespace TransactionStore.API.Extensions
{
    public class AdvancedController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRequestHelper _requestHelper;
        public AdvancedController(IConfiguration configuration, IRequestHelper requestHelper)
        {
            _configuration = configuration;
            _requestHelper = requestHelper;
        }

        public async Task CheckMicroservice(params Microservice[] service)
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var identity = await _requestHelper
                .SendRequestCheckValidateToken(_configuration[Microservice.MarvelousAuth.ToString()],
                AuthEndpoints.ApiAuth + AuthEndpoints.ValidationMicroservice, token);

            if (!service.Select(r => r.ToString()).Contains(identity.Data.IssuerMicroservice))
            {
                throw new ForbiddenException($"{identity.Data.IssuerMicroservice} doesn't have access to this endpiont");
            }
        }
    }
}
