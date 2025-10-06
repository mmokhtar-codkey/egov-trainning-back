using Training.Application.Services.Base;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Action;
using Training.Common.DTO.Lookup.Action.Parameters;
using Training.Domain;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Training.Application.Services.Lookups.Action
{
    public class ActionService : BaseService<Domain.Entities.Lookups.Action, AddActionDto, EditActionDto, ActionDto, Guid, Guid?>, IActionService
    {
        public ActionService(IServiceBaseParameter<Domain.Entities.Lookups.Action> parameters) : base(parameters)
        {

        }



        public async Task<PagedResult<IEnumerable<ActionDto>>> GetAllPagedAsync(BaseParam<ActionFilter> filter)
        {
            var limit = filter.PageSize;

            var offset = --filter.PageNumber * filter.PageSize;

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: PredicateBuilderFunction(filter.Filter), pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Action>, IEnumerable<ActionDto>>(query.Item2);

            return PagedResult<IEnumerable<ActionDto>>.Success(data, filter.PageNumber, filter.PageSize, query.Item1, MessagesConstants.Success);
        }

        public async Task<PagedResult<IEnumerable<ActionDto>>> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter)
        {

            var limit = filter.PageSize;

            var offset = ((--filter.PageNumber) * filter.PageSize);

            var predicate = DropDownPredicateBuilderFunction(filter.Filter);

            var query = await UnitOfWork.Repository.FindPagedAsync(predicate: predicate, pageNumber: offset, pageSize: limit, filter.OrderByValue);

            var data = Mapper.Map<IEnumerable<Domain.Entities.Lookups.Action>, IEnumerable<ActionDto>>(query.Item2);

            return PagedResult<IEnumerable<ActionDto>>.Success(data, filter.PageNumber, filter.PageSize, query.Item1, MessagesConstants.Success);

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

        static Expression<Func<Domain.Entities.Lookups.Action, bool>> PredicateBuilderFunction(ActionFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Action>(x => x.IsDeleted == filter.IsDeleted);
            if (!string.IsNullOrWhiteSpace(filter?.Name))
            {
                predicate = predicate.And(b => b.CreatedByEmployeeAr.Contains(filter.Name));
            }
            return predicate;
        }

        static Expression<Func<Domain.Entities.Lookups.Action, bool>> DropDownPredicateBuilderFunction(SearchCriteriaFilter filter)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Lookups.Action>(true);
            if (!string.IsNullOrWhiteSpace(filter?.SearchCriteria))
            {
                predicate = predicate.And(b => b.Name.Contains(filter.SearchCriteria));
            }
            return predicate;
        }

    }
}
