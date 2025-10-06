using Asp.Versioning;
using Training.Api.Controllers.V1.Base;
using Training.Application.Services.Lookups.Action;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Action;
using Training.Common.DTO.Lookup.Action.Parameters;
using Training.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Training.Api.Controllers.V1.Lookups
{
    /// <summary>
    /// Actions Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ActionsController : BaseController
    {
        private readonly IActionService _actionService;
        /// <summary>
        /// Constructor
        /// </summary>
        public ActionsController(IActionService actionService)
        {
            _actionService = actionService;
        }
        /// <summary>
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ActionDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<ActionDto>), 404)]
        public async Task<ActionResult<ApiResponse<ActionDto>>> GetAsync(int id)
        {
            var result = await _actionService.GetByIdAsync(id);
            return result.ToActionResult();
        }


        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ActionDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ActionDto>>), 400)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ActionDto>>>> GetAllAsync()
        {
            var result = await _actionService.GetAllAsync();
            return result.ToActionResult();
        }


        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        [ProducesResponseType(typeof(ApiPagedResponse<IEnumerable<ActionDto>>), 200)]
        [ProducesResponseType(typeof(ApiPagedResponse<IEnumerable<ActionDto>>), 400)]
        public async Task<ActionResult<ApiPagedResponse<IEnumerable<ActionDto>>>> GetPagedAsync([FromBody] BaseParam<ActionFilter> filter)
        {
            var result = await _actionService.GetAllPagedAsync(filter);
            return result.ToActionResult();
        }

    }
}
