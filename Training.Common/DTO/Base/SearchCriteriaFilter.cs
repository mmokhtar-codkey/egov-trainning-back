using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Base
{
    [ExcludeFromCodeCoverage]
    public class SearchCriteriaFilter
    {
        public string SearchCriteria { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
