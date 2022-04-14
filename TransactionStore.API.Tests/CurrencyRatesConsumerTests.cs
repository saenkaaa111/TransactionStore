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
    public class CurrencyRatesConsumerTests
    {
        private CurrencyRatesConsumer _consumer;
        private Mock<ICurrencyRatesService> _currencyRatesServiceMock;
        private Mock<ILogger<CurrencyRatesConsumer>> _logger;

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
            _logger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("CurrencyRatesExchangeModel recieved", o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }
    }
}
