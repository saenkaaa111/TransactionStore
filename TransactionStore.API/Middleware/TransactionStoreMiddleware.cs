using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using TransactionStore.API.Models.Response;
using TransactionStore.BusinessLayer;

namespace TransactionStore.API.Middleware
{
    public class TransactionStoreMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionStoreMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException)
            {
                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, "Сервер недоступен");
            }
            catch (NullReferenceException)
            {
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, "Не найдено");
            }
            catch (InsufficientFundsException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
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
