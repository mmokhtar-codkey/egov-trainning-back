using Training.Application.Services.Base;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Action;
using Training.Common.DTO.Lookup.Action.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Training.Application.Services.Lookups.Action
{
    public interface IActionService : IBaseService<Domain.Entities.Lookups.Action, AddActionDto, EditActionDto, ActionDto, Guid, Guid?>
    {
        Task<PagedResult<IEnumerable<ActionDto>>> GetAllPagedAsync(BaseParam<ActionFilter> filter);

        Task<PagedResult<IEnumerable<ActionDto>>> GetDropDownAsync(BaseParam<SearchCriteriaFilter> filter);

        Task<Result> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<Result> DeleteBulkAsync(IEnumerable<Guid> ids);
    }
}
