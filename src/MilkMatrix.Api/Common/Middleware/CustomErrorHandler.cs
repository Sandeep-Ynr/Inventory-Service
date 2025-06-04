using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using MilkMatrix.Domain.Entities.Responses;
using Serilog;
using static MilkMatrix.Api.Common.Constants.Constants;
using ILogger = Serilog.ILogger;

namespace MilkMatrix.Api.Common.Middleware
{
    public class CustomErrorHandler
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        public CustomErrorHandler(
            RequestDelegate next,
            ILogger logger,
            IDiagnosticContext diagnosticContext)
        {
            this._next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await SplunkLogging(httpContext.Request);
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task SplunkLogging(HttpRequest request)
        {
            var requestHeader = request?.Headers?.Select(x => x.Key + ":" + x.Value).ToArray();
            var requestBodyContent = await ReadRequestBody(request);

            _diagnosticContext.Set("Request Header", requestHeader != null && requestHeader.Any() ? string.Join(" && ", requestHeader) : "");
            _diagnosticContext.Set("Request Body", requestBodyContent);
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.Error(ex, ErrorMessage.GenericException);

            const int code = (int)HttpStatusCode.InternalServerError;
            var type = ex.GetType();
            var props = new List<PropertyInfo>(type.GetProperties());
            var status = props.FirstOrDefault(x => x.Name == AppConstants.StatusCode)?.GetValue(ex, null);
            string result;
            if (status != null)
            {
                var httpStatus = (int)(HttpStatusCode)status;
                result = JsonSerializer.Serialize(new ErrorResponse
                {
                    StatusCode = httpStatus,
                    ErrorMessage = ex.Message
                });
                context.Response.StatusCode = httpStatus;
            }
            //Amazon.Runtime.ErrorType
            else
            {
                result = JsonSerializer.Serialize(new ErrorResponse
                {
                    StatusCode = code,
                    ErrorMessage = ex.Message
                });
                context.Response.StatusCode = code;
            }
            context.Response.ContentType = AppConstants.ContentType;

            //Logging is being handled by serilog
            return context.Response.WriteAsync(result);
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            if (request.ContentLength is null or <= 0)
            {
                return string.Empty;
            }
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer).ConfigureAwait(false);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return bodyAsText;
        }
    }
}
