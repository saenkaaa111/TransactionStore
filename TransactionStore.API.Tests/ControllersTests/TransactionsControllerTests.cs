using AutoMapper;
using FluentValidation;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.API.Configuration;
using TransactionStore.API.Controllers;
using TransactionStore.API.Producers;
using TransactionStore.API.Tests.TestCaseSource;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class TransactionsControllerTests : VerifyLoggerHelper<TransactionsController>
    {
        private Mock<ITransactionService>? _transactionServiceMock;
        private TransactionsController? _transactionsController;
        private Mock<IRequestHelper>? _requestHelperMock;
        private Mock<ITransactionProducer>? _transactionProducer;
        private Mock<IConfiguration>? _configurationMock;
        private IValidator<TransactionRequestModel>? _validator;
        private IMapper? _mapper;
        private DefaultHttpContext? _defaultHttpContext;

        [SetUp]
        public void Setup()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapper>()));
            _logger = new Mock<ILogger<TransactionsController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _configurationMock = new Mock<IConfiguration>();
            _transactionProducer = new Mock<ITransactionProducer>();
            //_validator = new IValidator();
            _transactionsController = new TransactionsController(_transactionServiceMock.Object, 
                _mapper, _logger.Object, _transactionProducer.Object, 
                _requestHelperMock.Object, _configurationMock.Object,
                _validator);
            _defaultHttpContext = new DefaultHttpContext();
            _transactionsController.ControllerContext.HttpContext = _defaultHttpContext;
            _defaultHttpContext.Request.Headers.Authorization = "Token";
        }

        [Test]
        public async Task AddServicePayment_ValidRequestReceived_ReturnsStatusCode200()
        {
            //given
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            var transactionRequestModel = new TransactionRequestModel
            {
                AccountId = 1,
                Amount = 100,
                Currency = Currency.RUB,
            };
            var transactionId = 1;
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _transactionsController.ControllerContext.HttpContext = context;
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(transactionId);

            //when
            var actual = await _transactionsController.AddServicePayment(transactionRequestModel);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.Withdraw(It.IsAny<TransactionModel>()), Times.Once);
            LoggerVerify("Request to add Service payment in the conroller", LogLevel.Information);
            LoggerVerify($"Service payment with Id = {transactionId} added", LogLevel.Information);
        }

        [Test]
        public void AddServicePayment_Forbidden_ReturnsStatusCode200()
        {
            //given
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousCrm.ToString()
            };
            var transactionRequestModel = new TransactionRequestModel
            {
                AccountId = 1,
                Amount = 100,
                Currency = Currency.RUB,
            };
            var transactionId = 1;
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _transactionsController.ControllerContext.HttpContext = context;
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(transactionId);
            var expectedMessage = "MarvelousCrm doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.AddServicePayment(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to add Service payment in the conroller", LogLevel.Information);
        }

        [Test]
        public void AddServicePayment_NotValidModelReceived_ShouldThrowValidationException()
        {
            //given
            var currency = -1;
            var identityResponseModel = new IdentityResponseModel()
            {
                Id = 1,
                Role = "role",
                IssuerMicroservice = Microservice.MarvelousResource.ToString()
            };
            var transactionRequestModel = new TransactionRequestModel
            {
                AccountId = -1,
                Amount = -1,
                Currency = (Currency)currency,
            };
            var transactionId = 1;
            string token = "token";
            var context = new DefaultHttpContext();
            context.Request.Headers.Authorization = token;
            _transactionsController.ControllerContext.HttpContext = context;
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(transactionId);
            var expectedMessage = "TransactionRequestModel isn't valid";

            //when
            ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
           _transactionsController.AddServicePayment(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Error: TransactionRequestModel isn't valid", LogLevel.Error);
        }










        //[TestCaseSource(typeof(AddTransfer_ValidRequestReceived_TestCaseSource))]
        //public async Task AddTransfer_ValidRequestReceived_ReturnsStatusCode200(
        //    TransferRequestModel transferRequestModel, TransferModel transferModel,
        //    TransactionModel transactionModelFirst, TransactionModel transactionModelSecond,
        //    List<long> transferIds)
        //{
        //    //given
        //    _transactionServiceMock.Setup(t => t.AddTransfer(transferModel)).ReturnsAsync(transferIds);
        //    _transactionServiceMock.Setup(t => t.GetTransactionById(transferIds[0])).ReturnsAsync(transactionModelFirst);
        //    _transactionServiceMock.Setup(t => t.GetTransactionById(transferIds[1])).ReturnsAsync(transactionModelSecond);

        //    //when
        //    var result = await _transactionsController.AddTransfer(transferRequestModel);

        //    //then
        //    Assert.IsInstanceOf<ObjectResult>(result);
        //    _transactionServiceMock.Verify(t => t.AddTransfer(transferModel), Times.Once);
        //    LoggerVerify("Request to add Service payment in the conroller", LogLevel.Information);
        //    //LoggerVerify($"Service payment with Id = {transactionId} added", LogLevel.Information);
        //}
    }
}
