using Training.Common.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Training.Common.DTO.Base
{
    [ExcludeFromCodeCoverage]
    public class BaseParam<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public T Filter { get; set; }

        public IEnumerable<SortModel> OrderByValue { get; set; }
    }
}
