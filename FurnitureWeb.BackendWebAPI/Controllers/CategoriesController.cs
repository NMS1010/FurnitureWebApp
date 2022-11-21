using Domain.Entities;
using FurnitureWeb.Services.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryService;

        public CategoriesController(ICategoryServices categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] CategoryGetPagingRequest request)
        {
            var categories = await _categoryService.RetrieveAll(request);
            if (categories == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot get categories list"));
            return Ok(CustomAPIResponse<PagedResult<CategoryViewModel>>.Success(categories, StatusCodes.Status200OK));
        }

        [HttpGet("all/parent-categories")]
        public async Task<IActionResult> RetrieveParentCategories()
        {
            var categories = await _categoryService.GetParentCategory();
            if (categories == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot get parent categories list"));
            return Ok(CustomAPIResponse<PagedResult<CategoryViewModel>>.Success(categories, StatusCodes.Status200OK));
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> RetrieveById(int categoryId)
        {
            var category = await _categoryService.RetrieveById(categoryId);
            if (category == null)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot find this caterogy"));
            return Ok(CustomAPIResponse<CategoryViewModel>.Success(category, StatusCodes.Status200OK));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryId = await _categoryService.Create(request);
            if (categoryId <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot create this caterogy"));
            var category = await _categoryService.RetrieveById(categoryId);

            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var count = await _categoryService.Update(request);
            if (count <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot update this caterogy"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            int records = await _categoryService.Delete(categoryId);
            if (records <= 0)
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this caterogy"));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }
    }
}