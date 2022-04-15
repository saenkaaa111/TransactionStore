using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using TransactionStore.API.Controllers;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class BalanceControllerTests
    {
        private Mock<IBalanceService> _balanceServiceMock;
        private Mock<ILogger<BalanceController>> _loggerMock;
        private BalanceController _balanceController;
        private Mock<IRequestHelper> _requestHelperMock;

        [SetUp]
        public void Setup()
        {
            _balanceServiceMock = new Mock<IBalanceService>();
            _loggerMock = new Mock<ILogger<BalanceController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _balanceController = new BalanceController(_balanceServiceMock.Object, _loggerMock.Object, _requestHelperMock.Object, null);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_Returns200()
        {
            //given
            var ids = new List<int> { 1 };
            var balance = 777m;
            _balanceServiceMock.Setup(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB)).ReturnsAsync(balance);

            //when
            var result = _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB).Result;
            var okResponse = result as ObjectResult;

            //then
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, okResponse!.StatusCode);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrency_Forbidden_Returns403()
        {
            //given
            var ids = new List<int> { 1 };
            var balance = 777m;
            _balanceServiceMock.Setup(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB)).ReturnsAsync(balance);

            //when
            var result = _balanceController.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB).Result;
            var forbiddenResponse = result as ObjectResult;

            //then
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status403Forbidden, forbiddenResponse!.StatusCode);
        }
    }
}
