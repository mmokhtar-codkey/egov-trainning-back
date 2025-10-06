using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Configurations.Http
{
    [ExcludeFromCodeCoverage]
    public record MicroServicesEndpointUrl
    {
        public IdentityServerApiEndpoints IdentityServerApiEndpoints { get; set; }

        public FileManagerApiEndpoints FileManagerApiEndpoints { get; set; }

        public UserManagementApiEndpoints UserManagementApiEndpoints { get; set; }

        public SmsApiEndpoints SmsApiEndpoints { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public record IdentityServerApiEndpoints
    {
        [Required]
        public string GenerateClientCredentialToken { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public record LeaveManagementApiEndpoints
    {
        [Required]
        public string GetEmployeeLeaves { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public record UserManagementApiEndpoints
    {
        [Required]
        public string GetEmployeeByIdFromUserManager { get; set; } = string.Empty;

    }

    [ExcludeFromCodeCoverage]
    public record FileManagerApiEndpoints
    {
        [Required]
        public string Download { get; set; } = string.Empty;

        [Required]
        public string DownloadWithAppCode { get; set; } = string.Empty;

        [Required]
        public string GenerateToken { get; set; } = string.Empty;

        [Required]
        public string GenerateTokenWithClaims { get; set; } = string.Empty;

        [Required]
        public string DeleteMultiple { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public record SmsApiEndpoints
    {
        [Required]
        public string SendSms { get; set; } = string.Empty;
    }

}
