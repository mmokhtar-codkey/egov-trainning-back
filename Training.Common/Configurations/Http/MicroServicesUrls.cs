using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Configurations.Http
{
    [ExcludeFromCodeCoverage]
    public class MicroServicesUrls
    {
        private readonly IOptions<MicroServicesEndpointUrl> _microServicesEndpointUrl;
        public MicroServicesUrls(IOptions<MicroServicesEndpointUrl> microServicesEndpointUrl)
        {
            _microServicesEndpointUrl = microServicesEndpointUrl;
        }

        /* Identity Service Urls */
        public string GenerateClientCredentialToken => _microServicesEndpointUrl.Value.IdentityServerApiEndpoints.GenerateClientCredentialToken;


        /* File Manager Service Urls */
        public string GenerateTokenWithClaims => _microServicesEndpointUrl.Value.FileManagerApiEndpoints.GenerateTokenWithClaims;

        public string DownloadFileWithAppCode => _microServicesEndpointUrl.Value.FileManagerApiEndpoints.DownloadWithAppCode;

        /* SMS Api Service Urls */
        public string SendSms => _microServicesEndpointUrl.Value.SmsApiEndpoints.SendSms;

        /* User Manager Api  Service Urls */
        public string GetEmployeeByIdFromUserManager => _microServicesEndpointUrl.Value.UserManagementApiEndpoints.GetEmployeeByIdFromUserManager;

    }
}
