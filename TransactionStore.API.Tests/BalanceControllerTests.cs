using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.API.Controllers;
using TransactionStore.API.Tests.TestCaseSource;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class BalanceControllerTests : VerifyLoggerHelper<BalanceController>
    {
        private Mock<IBalanceService> _balanceServiceMock;
        private BalanceController _balanceController;
        private Mock<IRequestHelper> _requestHelperMock;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            _balanceServiceMock = new Mock<IBalanceService>();
            _logger = new Mock<ILogger<BalanceController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _configurationMock = new Mock<IConfiguration>();
            _balanceController = new BalanceController(_balanceServiceMock.Object, _logger.Object,
                _requestHelperMock.Object, _configurationMock.Object);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_TestCaseSource))]
        public async Task GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_ReturnsStatusCode200(
            IdentityResponseModel identityResponseModel, List<int> ids, decimal balance)
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _balanceController.ControllerContext.HttpContext = context;
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _balanceServiceMock.Setup(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB)).ReturnsAsync(balance);

            //when
            var result = await _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB);

            //then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ObjectResult>(result);
            _balanceServiceMock.Verify(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB), Times.Once);
            LoggerVerify("Request to receive a balance by AccountIds in the controller", LogLevel.Information);
            LoggerVerify("Balance received", LogLevel.Information);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrency_Forbidden_ShouldThrowForbiddenException()
        {
            //given
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _balanceController.ControllerContext.HttpContext = context;
            var ids = new List<int> { 1 };
            var balance = 777m;
            var expectedMessage = "MarvelousFrontendResource doesn't have access to this endpiont";
            IdentityResponseModel identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousFrontendResource.ToString()
            }; 
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                 It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _balanceServiceMock.Setup(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB)).ReturnsAsync(balance);

            //then
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB));

            // then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to receive a balance by AccountIds in the controller", LogLevel.Information);
        }
    }
}
