using Training.Common.DTO.Base;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Lookup.Status
{
    [ExcludeFromCodeCoverage]
    public class EditStatusDto : LookupDto<int?>
    {
        public string EntityName { get; set; }

        public string CssClass { get; set; }
    }
}
