using Training.Common.Extensions;
using Serilog;
using Serilog.Events;
using System.Reflection;

namespace Training.Api
{
    /// <summary>
    /// Start Point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Configuration Properties
        /// </summary>
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        /// <summary>
        /// Kick Off
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var applicationVersion = GetApplicationVersion();
            var applicationName = Configuration["ApplicationName"];
            Log.Logger = BaseLoggerConfiguration
                .CreateLoggerConfiguration(applicationName, applicationVersion)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteToSql(Configuration["LoggingDbConnectionString"])
                //.WriteToSeq(Configuration["SeqServerUrl"])
                .CreateLogger();

            try
            {
                Log.Information("-----Starting web host at {0} ------", applicationName + "-" + applicationVersion);
                var host = CreateHostBuilder(args).Build();
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly at {0}", applicationName + "-" + applicationVersion);
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
        /// <summary>
        /// Web Host Builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>();
                }).UseSerilog();
        private static string GetApplicationVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (version != null)
            {
                var plusIndex = version.IndexOf('+');
                if (plusIndex > 0)
                {
                    version = version.Substring(0, plusIndex);
                }
            }
            return version ?? "1.0.0"; // Default version if not found
        }
    }
}
