using Training.Api.MiddleWares.Swagger;
using System.Diagnostics.CodeAnalysis;

namespace Training.Api.MiddleWares
{
    /// <summary>
    /// Register Middleware
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureCustomMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LanguageMiddleware>();
            app.UseMiddleware<SwaggerBasicAuthMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
