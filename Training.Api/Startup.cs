using Asp.Versioning.ApiExplorer;
using Training.Api.Extensions;
using Training.Api.MiddleWares;
using Training.Application.DependencyExtension;

namespace Training.Api
{
    /// <summary>
    /// Start Up Class
    /// </summary>
    public class Startup
    {
        private readonly Shell _shell;
        /// <summary>
        /// Constructor
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            _shell = ((Shell)Activator.CreateInstance(typeof(Shell)))!;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        /// <summary>
        /// Public Configuration Property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configure Dependencies
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterServices(Configuration);

        }

        /// <summary>
        /// Configure Pipeline
        /// </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            _shell.ConfigureHttp(app, env);
            Shell.Start(_shell);
            app.Configure(Configuration, provider);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureCustomMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}