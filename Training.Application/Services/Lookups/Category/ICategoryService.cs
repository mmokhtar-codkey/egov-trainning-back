using Training.Application.Services.Base;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Category;
using Training.Common.DTO.Lookup.Category.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Training.Application.Services.Lookups.Category
{
    public interface ICategoryService : IBaseService<Domain.Entities.Lookups.Category, AddCategoryDto, EditCategoryDto, CategoryDto, Guid, Guid?>
    {
        Task<PagedResult<IEnumerable<CategoryDto>>> GetAllPagedAsync(BaseParam<CategoryFilter> filter);

        Task<PagedResult<IEnumerable<CategoryDto>>> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

        Task<Result> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<Result> DeleteBulkAsync(IEnumerable<Guid> ids);
    }
}
