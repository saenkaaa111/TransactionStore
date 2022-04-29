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
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.API.Configuration;
using TransactionStore.API.Controllers;
using TransactionStore.API.Producers;
using TransactionStore.API.Tests.TestCaseSource;
using TransactionStore.API.Validators;
using TransactionStore.BusinessLayer.Exceptions;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;
using TransactionStore.DataLayer.Entities;

namespace TransactionStore.API.Tests
{
    public class TransactionsControllerTests : VerifyLoggerHelper<TransactionsController>
    {
        private Mock<ITransactionService> _transactionServiceMock;
        private TransactionsController _transactionsController;
        private Mock<IRequestHelper> _requestHelperMock;
        private Mock<ITransactionProducer> _transactionProducerMock;
        private Mock<IConfiguration> _configurationMock;
        private IMapper _mapper;
        private DefaultHttpContext _defaultHttpContext;
        private TransactionRequestModelValidator _transactionRequestModelValidator;
        private TransferRequestModelValidator _transferRequestModelValidator;

        [SetUp]
        public void Setup()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapper>()));
            _logger = new Mock<ILogger<TransactionsController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _configurationMock = new Mock<IConfiguration>();
            _transactionProducerMock = new Mock<ITransactionProducer>();
            _transactionRequestModelValidator = new TransactionRequestModelValidator();
            _transferRequestModelValidator = new TransferRequestModelValidator();
            _transactionsController = new TransactionsController(_transactionServiceMock.Object,
                _mapper, _logger.Object, _transactionProducerMock.Object,
                _requestHelperMock.Object, _configurationMock.Object,
                _transactionRequestModelValidator, _transferRequestModelValidator);
            _defaultHttpContext = new DefaultHttpContext();
            _transactionsController.ControllerContext.HttpContext = _defaultHttpContext;
            _defaultHttpContext.Request.Headers.Authorization = "Token";
        }

        [TestCaseSource(typeof(AddTransaction_ValidRequestReceived_TestCaseSource))]
        public async Task AddDeposit_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long expected)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.AddDeposit(It.IsAny<TransactionModel>())).ReturnsAsync(expected);

            //when
            var actual = await _transactionsController.AddDeposit(transactionRequestModel);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.AddDeposit(It.IsAny<TransactionModel>()), Times.Once);
            _transactionProducerMock.Verify(t => t.NotifyTransactionAdded(It.IsAny<TransactionModel>()), Times.Once);
            LoggerVerify("Request to add Deposit in the controller", LogLevel.Information);
            LoggerVerify($"Deposit with Id = {expected} added", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransaction_Forbidden_TestCaseSource))]
        public void AddDeposit_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousResource doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.AddDeposit(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to add Deposit in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransaction_NotValidModelReceived_TestCaseSource))]
        public void AddDeposit_NotValidModelReceived_ShouldThrowValidationException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long transactionId)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.AddDeposit(It.IsAny<TransactionModel>())).ReturnsAsync(transactionId);
            var expectedMessage = "TransactionRequestModel isn't valid";

            //when
            ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
           _transactionsController.AddDeposit(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Error: TransactionRequestModel isn't valid", LogLevel.Error);
        }

        [TestCaseSource(typeof(AddTransfer_ValidRequestReceived_TestCaseSource))]
        public async Task AddTransfer_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, TransferRequestModel transferRequestModel,
            TransactionModel transactionModelFirst, TransactionModel transactionModelSecond, List<long> transferIds)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.AddTransfer(It.IsAny<TransferModel>())).ReturnsAsync(transferIds);
            _transactionServiceMock.Setup(t => t.GetTransactionById(transferIds[0])).ReturnsAsync(transactionModelFirst);
            _transactionServiceMock.Setup(t => t.GetTransactionById(transferIds[1])).ReturnsAsync(transactionModelSecond);

            //when
            var actual = await _transactionsController.AddTransfer(transferRequestModel);

            //then
            Assert.IsInstanceOf<ObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.AddTransfer(It.IsAny<TransferModel>()), Times.Once);
            _transactionProducerMock.Verify(p => p.NotifyTransactionAdded(transactionModelFirst), Times.Once);
            _transactionProducerMock.Verify(p => p.NotifyTransactionAdded(transactionModelSecond), Times.Once);
            LoggerVerify("Request to add Transfer in the controller", LogLevel.Information);
            LoggerVerify("Transfer added", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransfer_Forbidden_TestCaseSource))]
        public async Task AddTransfer_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, TransferRequestModel transferRequestModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousResource doesn't have access to this endpiont";
            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.AddTransfer(transferRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to add Transfer in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransfer_NotValidModelReceived_TestCaseSource))]
        public async Task AddTransfer_NotValidModelReceived_ShouldThrowValidationException(
            IdentityResponseModel identityResponseModel, TransferRequestModel transferRequestModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "TransferRequestModel isn't valid";

            //when
            ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
           _transactionsController.AddTransfer(transferRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Error: TransferRequestModel isn't valid", LogLevel.Error);
        }

        [TestCaseSource(typeof(AddTransaction_ValidRequestReceived_TestCaseSource))]
        public async Task Withdraw_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long expected)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(expected);

            //when
            var actual = await _transactionsController.Withdraw(transactionRequestModel);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.Withdraw(It.IsAny<TransactionModel>()), Times.Once);
            _transactionProducerMock.Verify(t => t.NotifyTransactionAdded(It.IsAny<TransactionModel>()), Times.Once);
            LoggerVerify("Request to add Withdraw in the controller", LogLevel.Information);
            LoggerVerify($"Withdraw with Id = {expected} added", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransaction_Forbidden_TestCaseSource))]
        public void Withdraw_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousResource doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.Withdraw(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to add Withdraw in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddTransaction_NotValidModelReceived_TestCaseSource))]
        public void Withdraw_NotValidModelReceived_ShouldThrowValidationException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long transactionId)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(transactionId);
            var expectedMessage = "TransactionRequestModel isn't valid";

            //when
            ValidationException? exception = Assert.ThrowsAsync<ValidationException>(() =>
           _transactionsController.Withdraw(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Error: TransactionRequestModel isn't valid", LogLevel.Error);
        }

        [TestCaseSource(typeof(GetTransactionsByAccountIds_ValidRequestReceived_TestCaseSource))]
        public async Task GetTransactionsByAccountIds_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, List<int> ids, ArrayList transactions)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.GetTransactionsByAccountIds(ids)).ReturnsAsync(transactions);

            //when
            var actual = await _transactionsController.GetTransactionsByAccountIds(ids);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.GetTransactionsByAccountIds(ids), Times.Once);
            LoggerVerify("Request to receive all transactions by AccountIds in the controller", LogLevel.Information);
            LoggerVerify("Transactions by AccountIds received", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetTransactionsByAccountIds_Forbidden_TestCaseSource))]
        public async Task GetTransactionsByAccountIds_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, List<int> ids)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousResource doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.GetTransactionsByAccountIds(ids));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to receive all transactions by AccountIds in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetTransactionById_ValidRequestReceived_TestCaseSource))]
        public async Task GetTransactionById_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, long id, TransactionModel transaction)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.GetTransactionById(id)).ReturnsAsync(transaction);

            //when
            var actual = await _transactionsController.GetTransactionById(id);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.GetTransactionById(id), Times.Once);
            LoggerVerify($"Request to receive transaction by Id = {id} in the controller", LogLevel.Information);
            LoggerVerify($"Transaction by AccountId = {id} received", LogLevel.Information);
        }

        [TestCaseSource(typeof(GetTransactionById_Forbidden_TestCaseSource))]
        public async Task GetTransactionById_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, long id)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousResource doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.GetTransactionById(id));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify($"Request to receive transaction by Id = {id} in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddServicePayment_ValidRequestReceived_TestCaseSource))]
        public async Task AddServicePayment_ValidRequestReceived_ShouldReturnStatusCode200(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long expected)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            _transactionServiceMock.Setup(t => t.Withdraw(It.IsAny<TransactionModel>())).ReturnsAsync(expected);

            //when
            var actual = await _transactionsController.AddServicePayment(transactionRequestModel);

            //then
            Assert.IsInstanceOf<OkObjectResult>(actual.Result);
            _transactionServiceMock.Verify(t => t.Withdraw(It.IsAny<TransactionModel>()), Times.Once);
            _transactionProducerMock.Verify(t => t.NotifyTransactionAdded(It.IsAny<TransactionModel>()), Times.Once);
            LoggerVerify("Request to add Service payment in the controller", LogLevel.Information);
            LoggerVerify($"Service payment with Id = {expected} added", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddServicePayment_Forbidden_TestCaseSource))]
        public void AddServicePayment_Forbidden_ShouldThrowForbiddenException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel)
        {
            //given
            _requestHelperMock.Setup(x => x.SendRequestCheckValidateToken(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(identityResponseModel);
            var expectedMessage = "MarvelousCrm doesn't have access to this endpiont";

            //when
            ForbiddenException? exception = Assert.ThrowsAsync<ForbiddenException>(() =>
            _transactionsController.AddServicePayment(transactionRequestModel));

            //then
            Assert.That(exception?.Message, Is.EqualTo(expectedMessage));
            LoggerVerify("Request to add Service payment in the controller", LogLevel.Information);
        }

        [TestCaseSource(typeof(AddServicePayment_NotValidModelReceived_TestCaseSource))]
        public void AddServicePayment_NotValidModelReceived_ShouldThrowValidationException(
            IdentityResponseModel identityResponseModel, TransactionRequestModel transactionRequestModel, long transactionId)
        {
            //given
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
    }
}
