using Training.Common.Core;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Identity.Role
{
    [ExcludeFromCodeCoverage]
    public class RoleDto : IEntityDto<long?>
    {
        public long? Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string AppNameEn { get; set; }
        public string AppNameAr { get; set; }
        public string AppCode { get; set; }
        public string Code { get; set; }
    }
}
