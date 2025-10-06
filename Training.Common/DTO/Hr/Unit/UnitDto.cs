using Training.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Hr.Unit
{
    [ExcludeFromCodeCoverage]
    public class UnitDto
    {
        public long? Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public UnitType? UnitType { get; set; }
    }
}
