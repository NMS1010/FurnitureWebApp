using Domain.Entities;
using FurnitureWeb.Services.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return BadRequest();
            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> RetrieveById(int categoryId)
        {
            var category = await _categoryService.RetrieveById(categoryId);
            if (category == null)
                return BadRequest();
            return Ok(category);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryId = await _categoryService.Create(request);
            if (categoryId <= 0)
                return BadRequest();
            var category = await _categoryService.RetrieveById(categoryId);

            return CreatedAtAction(nameof(RetrieveById), new { categoryId = categoryId }, category);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var count = await _categoryService.Update(request);
            if (count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            int records = await _categoryService.Delete(categoryId);
            if (records <= 0)
                return BadRequest();
            return Ok();
        }
    }
}