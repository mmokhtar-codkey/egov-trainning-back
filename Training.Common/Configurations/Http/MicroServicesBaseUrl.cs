using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Configurations.Http
{
    [ExcludeFromCodeCoverage]
    public record MicroServicesBaseUrl
    {
        public string IdentityServerApiBaseUrl { get; set; } = string.Empty;

        public string FileManagerApiBaseUrl { get; set; } = string.Empty;

        public string UserManagementApiBaseUrl { get; set; } = string.Empty;

        public string SmsApiBaseUrl { get; set; } = string.Empty;

    }
}
