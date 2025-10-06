using System.Diagnostics.CodeAnalysis;

namespace Training.Common.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationSettings
    {
        public const string StsSection = "StsConfig";

        public const string MicroServicesBaseUrlsSection = "MicroServicesBaseUrls";

        public const string MicroServicesEndPointsSection = "MicroServicesEndPoints";

        public const string RabbitMqConfig = "RabbitMqConfig";
    }
}
