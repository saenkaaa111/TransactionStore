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

        protected async Task CheckMicroservice(params Microservice[] service)
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();
            var identity = await _requestHelper
                .SendRequestCheckValidateToken(_configuration[Microservice.MarvelousAuth.ToString()],
                AuthEndpoints.ApiAuth + AuthEndpoints.ValidationMicroservice, token);

            if (!service.Select(r => r.ToString()).Contains(identity.IssuerMicroservice))
            {
                throw new ForbiddenException($"{identity.IssuerMicroservice} doesn't have access to this endpiont");
            }
        }
    }
}
