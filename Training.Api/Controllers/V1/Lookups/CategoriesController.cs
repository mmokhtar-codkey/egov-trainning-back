using Asp.Versioning;
using Training.Api.Controllers.V1.Base;
using Training.Application.Services.Lookups.Category;
using Training.Common.Core;
using Training.Common.DTO.Base;
using Training.Common.DTO.Lookup.Category;
using Training.Common.DTO.Lookup.Category.Parameters;
using Training.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Training.Api.Controllers.V1.Lookups
{
    /// <summary>
    /// Categories Controller
    /// </summary>
    [ApiVersion("1.0")]
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
        /// Get By Id 
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 404)]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetAsync(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return result.ToActionResult();
        }


        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), 400)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAllAsync()
        {
            var result = await _categoryService.GetAllAsync();
            return result.ToActionResult();
        }

        /// <summary>
        /// GetAll Data paged
        /// </summary>
        /// <param name="filter">Filter responsible for search and sort</param>
        /// <returns></returns>
        [HttpPost("getPaged")]
        [ProducesResponseType(typeof(ApiPagedResponse<IEnumerable<CategoryDto>>), 200)]
        [ProducesResponseType(typeof(ApiPagedResponse<IEnumerable<CategoryDto>>), 400)]
        public async Task<ActionResult<ApiPagedResponse<IEnumerable<CategoryDto>>>> GetPagedAsync([FromBody] BaseParam<CategoryFilter> filter)
        {
            var result = await _categoryService.GetAllPagedAsync(filter);
            return result.ToActionResult();
        }
    }
}
