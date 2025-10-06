using Training.Application.Services.Base;
using Training.Application.Services.Lookups.Category;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Category;
using Training.Common.DTO.Lookup.Category.Parameters;
using Training.Domain;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Training.Application.Services.Posts.Category
{
    public class CategoryService : BaseService<Domain.Entities.Lookups.Category, AddCategoryDto, EditCategoryDto, CategoryDto, Guid, Guid?>, ICategoryService
    {
        public CategoryService(IServiceBaseParameter<Domain.Entities.Lookups.Category> parameters) : base(parameters)
        {

        }

        public async Task<PagedResult<IEnumerable<CategoryDto>>> GetAllPagedAsync(BaseParam<CategoryFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Category>, IEnumerable<CategoryDto>>(query.Item2);

            return PagedResult<IEnumerable<CategoryDto>>.Success(data, filter.PageNumber, filter.PageSize, query.Item1, MessagesConstants.Success);
        }

        public async Task<PagedResult<IEnumerable<CategoryDto>>> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = ((--filter.PageNumber) * filter.PageSize);

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Category>, IEnumerable<CategoryDto>>(query.Item2);

            return PagedResult<IEnumerable<CategoryDto>>.Success(data, filter.PageNumber, filter.PageSize, query.Item1, MessagesConstants.Success);

        }

        public async Task<Result> DeleteRangeAsync(IEnumerable<Guid> ids)
        {
            var entitiesToDelete = await UnitOfWork.Repository.FindAsync(d => ids.Contains(d.Id));

            UnitOfWork.Repository.RemoveRange(entitiesToDelete);

            var rows = await UnitOfWork.SaveChangesAsync();

            return Result.Success(MessagesConstants.DeleteSuccess);
        }

        public async Task<Result> DeleteBulkAsync(IEnumerable<Guid> ids)
        {
            var rows = await UnitOfWork.Repository.RemoveBulkAsync(x => ids.Contains(x.Id));

            if (rows > 0)
            {
                return Result.Success(MessagesConstants.DeleteSuccess);
            }
            return Result.Failure(MessagesConstants.DeleteError);
        }

        static Expression<Func<Domain.Entities.Lookups.Category, bool>> PredicateBuilderFunction(CategoryFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Category>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter?.Name))
            {
                predicate = predicate.And(b => b.CreatedByEmployeeAr.Contains(filter.Name));
            }
            return predicate;
        }

        static Expression<Func<Domain.Entities.Lookups.Category, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Category>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

    }
}
