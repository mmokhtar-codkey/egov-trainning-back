using Asp.Versioning;
using Training.Api.Controllers.V2.Base;
using Training.Application.Services.Lookups.Action;
using Training.Common.Core;
using Training.Common.DTO.Lookup.Action;
using Training.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Training.Api.Controllers.V2.Lookups
{
    /// <summary>
    /// Actions Controller
    /// </summary>
    [ApiVersion("2.0")]
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
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EditActionDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<EditActionDto>), 404)]
        public async Task<ActionResult<ApiResponse<EditActionDto>>> GetEditAsync(int id)
        {
            var result = await _actionService.GetByIdForEditAsync(id);
            return result.ToActionResult();
        }


        /// <summary>
        /// Add 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse<Guid?>), 201)]
        [ProducesResponseType(typeof(ApiResponse<Guid?>), 400)]
        public async Task<ActionResult<ApiResponse<Guid?>>> AddAsync([FromBody] AddActionDto dto)
        {
            var result = await _actionService.AddAsync(dto);
            return result.ToActionResult(HttpStatusCode.Created);
        }


        /// <summary>
        /// Update  
        /// </summary>
        /// <param name="model">Object content</param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse<Guid?>), 200)]
        [ProducesResponseType(typeof(ApiResponse<Guid?>), 400)]
        public async Task<ActionResult<ApiResponse<Guid?>>> UpdateAsync(AddActionDto model)
        {
            var result = await _actionService.UpdateAsync(model);
            return result.ToActionResult();
        }

        /// <summary>
        /// Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse>> DeleteAsync(int id)
        {
            var result = await _actionService.DeleteAsync(id);
            return result.ToActionResult();
        }


        /// <summary>
        /// Soft Remove  by id
        /// </summary>
        /// <param name="id">PK</param>
        /// <returns></returns>
        [HttpDelete("deleteSoft/{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<ApiResponse>> DeleteSoftAsync(int id)
        {
            var result = await _actionService.DeleteSoftAsync(id);
            return result.ToActionResult();
        }
    }
}
