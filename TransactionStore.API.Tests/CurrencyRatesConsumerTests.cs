using Marvelous.Contracts.ExchangeModels;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using TransactionStore.API.Consumers;
using TransactionStore.API.Tests.TestCaseSource;
using TransactionStore.BusinessLayer.Services;

namespace TransactionStore.API.Tests
{
    public class CurrencyRatesConsumerTests : VerifyLoggerHelper<CurrencyRatesConsumer>
    {
        private CurrencyRatesConsumer _consumer;
        private Mock<ICurrencyRatesService> _currencyRatesServiceMock;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CurrencyRatesConsumer>>();
            _currencyRatesServiceMock = new Mock<ICurrencyRatesService>();
            _consumer = new CurrencyRatesConsumer(_currencyRatesServiceMock.Object, _logger.Object);
        }

        [TestCaseSource(typeof(CurrencyRatesConsumerTestCaseSource))]
        public void Consume_ShouldReceiveCurrensyRates(CurrencyRatesExchangeModel currencyRatesExchangeModel)
        {
            //given
            var context = Mock.Of<ConsumeContext<CurrencyRatesExchangeModel>>(_ =>
             _.Message == currencyRatesExchangeModel);
            var messageGet = $"Getting Account {context.Message.Rates}";
            var messageRecive = $"Account {context.Message.Rates} recived";

            //when
            _consumer.Consume(context);

            //then
            _currencyRatesServiceMock.Verify(x => x.SaveCurrencyRates(currencyRatesExchangeModel), Times.Once);
            LoggerVerify("CurrencyRatesExchangeModel recieved", LogLevel.Information);
        }
    }
}
