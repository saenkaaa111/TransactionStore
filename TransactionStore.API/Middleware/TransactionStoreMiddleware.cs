using NLog;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using TransactionStore.API.Models;
using TransactionStore.BusinessLayer;

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
            catch (SqlException)
            {
                _logger.Debug("Exception: Сервер недоступен");

                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, "Сервер недоступен");
            }
            catch (InsufficientFundsException ex)
            {
                _logger.Debug($"Exception: {ex.Message}");

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
            var result = JsonSerializer.Serialize(new ErrorOutputModel
            {
                Message = message,
                StatusCode = (int)code,
                StatusCodeName = code.ToString()
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(result);
        }
    }
}
