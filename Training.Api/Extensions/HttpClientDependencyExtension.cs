using Training.Api.HttpDelegationHandlers;
using Training.Common.Configurations;
using Training.Common.Configurations.Http;
using Training.Common.DTO.Identity.Sts;
using Training.Integration.TokenRepository;
using Microsoft.Extensions.Options;

namespace Training.Api.Extensions
{
    /// <summary>
    /// Http Client Dependency Injection Extension And Security Options
    /// </summary>
    public static class HttpClientDependencyExtension
    {
        /// <summary>
        /// Register STS  (Secure Token Service) settings 
        /// </summary>
        public static void AddSecurityOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<StsConfig>()
                .BindConfiguration(ConfigurationSettings.StsSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        /// <summary>
        /// Register STS  (Secure Token Service) settings 
        /// </summary>
        public static void AddHttpClientsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<MicroServicesBaseUrl>()
                .BindConfiguration(ConfigurationSettings.MicroServicesBaseUrlsSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<MicroServicesEndpointUrl>()
                .BindConfiguration(ConfigurationSettings.MicroServicesEndPointsSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton<MicroServicesUrls>();
        }

        /// <summary>
        /// Add Http Clients
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(ClientNames.IdentityServerApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MicroServicesBaseUrl>>().Value;

                client.BaseAddress = new Uri(options.IdentityServerApiBaseUrl);
                //client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient(ClientNames.UserManagerApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MicroServicesBaseUrl>>().Value;

                client.BaseAddress = new Uri(options.UserManagementApiBaseUrl);
                //client.Timeout = TimeSpan.FromSeconds(30);
            }).AddHttpMessageHandler(sp => new AuthorizedHttpClientHandler(sp.GetRequiredService<ITokenRepository>()));

            services.AddHttpClient(ClientNames.FileManagerApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MicroServicesBaseUrl>>().Value;

                client.BaseAddress = new Uri(options.FileManagerApiBaseUrl);
                //client.Timeout = TimeSpan.FromSeconds(30);
            }).AddHttpMessageHandler(sp => new AuthorizedHttpClientHandler(sp.GetRequiredService<ITokenRepository>()));


            services.AddHttpClient(ClientNames.SmsApiClient, (serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MicroServicesBaseUrl>>().Value;

                client.BaseAddress = new Uri(options.SmsApiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });
        }
    }
}
