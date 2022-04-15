using AutoMapper;
using Marvelous.Contracts.Enums;
using Marvelous.Contracts.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.API.Configuration;
using TransactionStore.API.Controllers;
using TransactionStore.API.Producers;
using TransactionStore.API.Tests.TestCaseSource;
using TransactionStore.BusinessLayer.Helpers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class TransactionsControllerTests : VerifyLoggerHelper<TransactionsController>
    {
        private Mock<ITransactionService> _transactionServiceMock;
        private TransactionsController _transactionsController;
        private Mock<IRequestHelper> _requestHelperMock;
        private Mock<ITransactionProducer> _transactionProducer;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessMapper>()));
            _logger = new Mock<ILogger<TransactionsController>>();
            _requestHelperMock = new Mock<IRequestHelper>();
            _transactionProducer = new Mock<ITransactionProducer>();
            _transactionsController = new TransactionsController(_transactionServiceMock.Object, 
                _mapper, _logger.Object, _transactionProducer.Object, _requestHelperMock.Object, null);
        }

        //[TestCaseSource(typeof(AddTransfer_ValidRequestReceived_ReturnsStatusCode200_TestCaseSource))]
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
        //    LoggerVerify("Request to add Transfer in the controller", LogLevel.Information);
        //    LoggerVerify("Transfer added", LogLevel.Information);
        //}

    }
}
