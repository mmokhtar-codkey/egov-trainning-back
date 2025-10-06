using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Sms
{
    [ExcludeFromCodeCoverage]
    public class SmsDto
    {
        public string Message { get; set; }
        public string Phone { get; set; }
    }
}
