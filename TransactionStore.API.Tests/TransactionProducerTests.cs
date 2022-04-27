using Marvelous.Contracts.Enums;
using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using TransactionStore.API.Producers;
using TransactionStore.BusinessLayer.Models;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class TransactionProducerTests : VerifyLoggerHelper<TransactionProducer>
    {
        private Mock<IBus> _bus;
        private Mock<ICalculationService> _calculationServiceMock;
        private TransactionProducer _transactionProducer;

        [SetUp]
        public void SetUp()
        {
            _bus = new Mock<IBus>();
            _logger = new Mock<ILogger<TransactionProducer>>();
            _calculationServiceMock = new Mock<ICalculationService>();
            _transactionProducer = new TransactionProducer(_calculationServiceMock.Object, _logger.Object, _bus.Object);
        }

        [Test]
        public async Task NotifyTransactionAdded_ValidRequestReceived_ShouldPublishTransactionModel()
        {
            //given
            var transactionModel = new TransactionModel()
            {
                Id = 7,
                Type = TransactionType.Deposit,
                Amount = 777,
                AccountId = 7,
                Currency = Currency.RUB
            };

            //when
            await _transactionProducer.NotifyTransactionAdded(transactionModel);

            //then
            _bus.Verify(v => v.Publish(It.IsAny<TransactionExchangeModel>(), It.IsAny<CancellationToken>()), Times.Once);
            LoggerVerify($"Transaction with id = {transactionModel.Id} published", LogLevel.Information);
        }
    }
}
