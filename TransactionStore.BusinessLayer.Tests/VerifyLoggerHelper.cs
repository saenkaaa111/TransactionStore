using Microsoft.Extensions.Logging;
using Moq;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace TransactionStore.BusinessLayer.Tests
{
    public abstract class VerifyLoggerHelper<T>
    {
        public Mock<ILogger<T>> _logger;

        internal void LoggerVerify(string message, LogLevel logLevel)
        {
            _logger.Verify(
                x => x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
