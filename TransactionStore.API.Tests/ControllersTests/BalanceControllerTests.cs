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
using TransactionStore.DataLayer.Entities;
using TransactionStore.DataLayer.Repository;

namespace TransactionStore.API.Tests
{
    public class BalanceControllerTests : VerifyLoggerHelper<BalanceController>
    {
        private Mock<IBalanceService> _balanceServiceMock;
        private BalanceController _balanceController;
        private Mock<IRequestHelper> _requestHelperMock;
        private Mock<IConfiguration> _configurationMock;
        private DefaultHttpContext _defaultHttpContext;
        private Mock<ITransactionRepository> _transactionRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<BalanceController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _configurationMock = new Mock<IConfiguration>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _balanceServiceMock = new Mock<IBalanceService>();
            _balanceController = new BalanceController(_balanceServiceMock.Object, _logger.Object,
                _requestHelperMock.Object, _configurationMock.Object);
            _defaultHttpContext = new DefaultHttpContext();
            _balanceController.ControllerContext.HttpContext = _defaultHttpContext;
            _defaultHttpContext.Request.Headers.Authorization = "Token";
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_TestCaseSource))]
        public async Task GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_ReturnsStatusCode200(
            IdentityResponseModel identityResponseModel, List<int> ids, Currency currencyRub, List<TransactionDto> transactions)
        {
            //given
            _transactionRepositoryMock.Setup(t => t.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);

            //when
            var actual = await _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, currencyRub);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            LoggerVerify("Request to receive a balance by AccountIds in the controller", LogLevel.Information);
            LoggerVerify("Balance received", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrency_Forbidden_TestCaseSource))]
        public async Task GetBalanceByAccountIdsInGivenCurrency_Forbidden_ShouldThrowForbiddenException(
            string expectedMessage, IdentityResponseModel identityResponseModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                 It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);

            //then
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(async () => await
           _balanceController.GetBalanceByAccountIdsInGivenCurrency(It.IsAny<List<int>>(), It.IsAny<Currency>()));

            // then 
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to receive a balance by AccountIds in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetBalanceByAccountIdsInGivenCurrency_InvalidRequest_TestCaseSource))]
        public void GetBalanceByAccountIdsInGivenCurrency_InvalidRequest_ShouldThrowForbiddenException(
            List<int> ids, Currency invalidCurrency, string expectedMessage,
            IdentityResponseModel identityResponseModel, List<TransactionDto> transactions)
        {
            //given
            _balanceServiceMock.Setup(s => s.GetBalanceByAccountIdsInGivenCurrency(ids, invalidCurrency))
                .Throws(new CurrencyNotReceivedException("The request for the currency value was not received"));
            _transactionRepositoryMock.Setup(t => t.GetTransactionsByAccountIds(It.IsAny<List<int>>())).ReturnsAsync(transactions);
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);

            //then
            CurrencyNotReceivedException? exception = Assert.ThrowsAsync<CurrencyNotReceivedException>(() =>
            _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, invalidCurrency));

            //then 
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
        }
    }
}
