using FluentValidation;
using Marvelous.Contracts.ResponseModels;
using NLog;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using TransactionStore.BusinessLayer.Exceptions;

namespace TransactionStore.API.Middleware
{
    public class TransactionStoreMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;

        public TransactionStoreMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ForbiddenException ex)
            {
                _logger.Debug("Exception: Forbidden");

                await HandleExceptionAsync(context, HttpStatusCode.Forbidden, ex.Message);
            }
            catch (RequestTimeoutException ex)
            {
                _logger.Debug("Exception: Request Timeout");

                await HandleExceptionAsync(context, HttpStatusCode.RequestTimeout, ex.Message);
            }
            catch (ServiceUnavailableException ex)
            {
                _logger.Debug("Exception: Service Unavailable");

                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, ex.Message);
            }
            catch (InsufficientFundsException ex)
            {
                _logger.Debug($"Exception: {ex.Message}");

                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (CurrencyNotReceivedException ex)
            {
                _logger.Debug($"Exception: {ex.Message}");

                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (ValidationException ex)
            {
                _logger.Debug($"Exception: {ex.Message}");
                await HandleExceptionAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
            }
            catch (TransactionNotFoundException ex)
            {
                _logger.Debug($"Exception: {ex.Message}");

                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (DbTimeoutException ex)
            {
                _logger.Debug("Exception: Db Timeout. Flood crossing");

                await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Debug($"Exception: {ex.Message}");

                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
        {
            var result = JsonSerializer.Serialize(new ExceptionResponseModel
            {
                Message = message,
                Code = (int)code
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result);
        }
    }
}
