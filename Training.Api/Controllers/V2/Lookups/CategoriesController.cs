using Asp.Versioning;
using Training.Api.Controllers.V2.Base;
using Training.Application.Services.Lookups.Category;
using Training.Common.Core;
using Training.Common.DTO.Lookup.Category;
using Training.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Training.Api.Controllers.V2.Lookups
{
    /// <summary>
    /// Categories Controller
    /// </summary>
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        /// <summary>
        /// Constructor
        /// </summary>
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get For Edit 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getEdit/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EditCategoryDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<EditCategoryDto>), 404)]
        public async Task<ActionResult<ApiResponse<EditCategoryDto>>> GetEditAsync(int id)
        {
            var result = await _categoryService.GetByIdForEditAsync(id);
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
        public async Task<ActionResult<ApiResponse<Guid?>>> AddAsync([FromBody] AddCategoryDto dto)
        {
            var result = await _categoryService.AddAsync(dto);
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
        public async Task<ActionResult<ApiResponse<Guid?>>> UpdateAsync(AddCategoryDto model)
        {
            var result = await _categoryService.UpdateAsync(model);
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
            var result = await _categoryService.DeleteAsync(id);
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
            var result = await _categoryService.DeleteSoftAsync(id);
            return result.ToActionResult();
        }

    }
}
