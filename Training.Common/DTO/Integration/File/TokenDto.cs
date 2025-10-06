using System;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Integration.File
{
    [ExcludeFromCodeCoverage]
    public class TokenDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
    }
}
