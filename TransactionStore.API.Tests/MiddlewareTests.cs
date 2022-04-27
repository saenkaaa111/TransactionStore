using FluentValidation;
using Marvelous.Contracts.ResponseModels;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TransactionStore.API.Middleware;
using TransactionStore.BusinessLayer.Exceptions;

namespace TransactionStore.API.Tests
{
    public class MiddlewareTests
    {
        private DefaultHttpContext _defaultHttpContext;

        [SetUp]
        public void SetUp()
        {
            _defaultHttpContext = new DefaultHttpContext
            {
                Response = { Body = new MemoryStream() },
                Request = { Path = "/" }
            };
        }

        [Test]
        public async Task Invoke_ValidRequestReceived_ShouldResponse()
        {
            //given
            var expected = "Request handed over to next request delegate";
            var middleware = new TransactionStoreMiddleware(innerHttpContext =>
            {
                innerHttpContext.Response.WriteAsync(expected);
                return Task.CompletedTask;
            });

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldThrowForbiddenException()
        {
            //given
            var message = "Forbidden";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status403Forbidden, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new ForbiddenException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowRequestTimeoutException()
        {
            //given
            var message = "Request Timeout";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status408RequestTimeout, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new RequestTimeoutException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowInsufficientFundsException()
        {
            //given
            var message = "Insufficient Funds";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status409Conflict, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new InsufficientFundsException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowCurrencyNotReceivedException()
        {
            //given
            var message = "Currency not received";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status409Conflict, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new InsufficientFundsException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowValidationException()
        {
            //given
            var message = "";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status422UnprocessableEntity, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new ValidationException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowTransactionNotFoundException()
        {
            //given
            var message = "Transactions weren't found";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status404NotFound, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new TransactionNotFoundException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Invoke_ShouldTrowDbTimeoutException()
        {
            //given
            var message = "Flood crossing";
            var expected = GetJsonExceptionResponseModel(StatusCodes.Status409Conflict, message);
            var middleware = new TransactionStoreMiddleware(_ => throw new DbTimeoutException(message));

            //when
            await middleware.InvokeAsync(_defaultHttpContext);
            var actual = GetResponseBody();

            //then
            Assert.AreEqual(expected, actual);
        }

        private static string GetJsonExceptionResponseModel(int statusCode, string message) =>
            JsonSerializer.Serialize(new ExceptionResponseModel { Code = statusCode, Message = message });

        private string GetResponseBody()
        {
            _defaultHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            return new StreamReader(_defaultHttpContext.Response.Body).ReadToEnd();
        }
    }
}
