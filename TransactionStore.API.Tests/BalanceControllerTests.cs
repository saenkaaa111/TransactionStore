using Castle.Core.Configuration;
using Marvelous.Contracts.Enums;
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

        [SetUp]
        public void Setup()
        {
            _balanceServiceMock = new Mock<IBalanceService>();
            _loggerMock = new Mock<ILogger<BalanceController>>();
            _balanceController = new BalanceController(_balanceServiceMock.Object, _loggerMock.Object, null, null);
        }

        [Test]
        public void GetBalanceByAccountIdsInGivenCurrency_ValidRequestReceived_ReturnsBalance()
        {
            //given
            var ids = new List<int> { 1 };
            var balance = 777m;
            _balanceServiceMock.Setup(b => b.GetBalanceByAccountIdsInGivenCurrency(ids, Currency.RUB)).ReturnsAsync(balance);

            //when
            var result = _balanceController.Ok(balance);

            //then
            Assert.IsInstanceOf<ActionResult>(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }
    } 
}
