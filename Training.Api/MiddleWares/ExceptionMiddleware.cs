using Training.Common.Core;
using Training.Common.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Task = System.Threading.Tasks.Task;

namespace Training.Api.MiddleWares
{
    /// <summary>
    /// Exception Middleware
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExceptionMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handle
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {


            var serializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var exception = new
            {
                ex.Message,
                ex.StackTrace,
                ex.InnerException
            };

            var exceptionJson = JsonConvert.SerializeObject(exception, serializerSettings);

            context.Response.ContentType = "application/json";

            var detailedExceptionMessage = $"----------Exception---------{exceptionJson}---------";
            Log.ForContext("Message", exception.Message)
            .Error(detailedExceptionMessage);

            if (ex is BaseException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var response = new ApiResponse
                {
                    Message = ex.Message,
                    StatusCode = (int)HttpStatusCode.NotFound
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
            }
            else if (ex is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                var response = new ApiResponse
                {
                    Message = "Unauthorized",
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
            }
            else if (ex is DbUpdateException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                if (ex.InnerException != null)
                {
                    var dbException = (SqlException)ex.InnerException;

                    switch (dbException.Number)
                    {
                        case 547:
                            {
                                var table = dbException.Message.Split("table");
                                var column = table[1].Split("column");
                                var response = new ApiResponse
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = $"Wrong Foreign Key (Id) For Entity {column[0]}"
                                };

                                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
                                break;
                            }
                        default:
                            {
                                var response = new ApiResponse
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Message = dbException.Message
                                };
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
                                break;
                            }
                    }
                }
                else
                {
                    var response = new ApiResponse
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = ex.Message
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = _configuration["Enable_Stack_Trace"] == "TRUE" ? exceptionJson : ex.Message
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
            }
        }
    }
}


