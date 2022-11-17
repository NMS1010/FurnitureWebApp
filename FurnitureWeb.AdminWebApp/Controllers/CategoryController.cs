using FurnitureWeb.APICaller.Category;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/categories/")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryAPIClient _categoryAPIClient;

        public CategoryController(ICategoryAPIClient categoryAPIClient)
        {
            _categoryAPIClient = categoryAPIClient;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            if (error)
                ViewData["Error"] = error;
            var res = await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest());
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            var res = await _categoryAPIClient.CreateCategory(request);
            return RedirectToAction(nameof(Index), res.IsSuccesss ? false : true);
        }

        [HttpGet("delete/{categoryId}")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var res = await _categoryAPIClient.DeleteCategory(categoryId);

            return RedirectToAction(nameof(Index), res.IsSuccesss ? false : true);
        }

        [Route("get/{categoryId}")]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var res = await _categoryAPIClient.GetCategoryById(categoryId);
            if (res == null || !res.IsSuccesss)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(CategoryUpdateRequest request)
        {
            var res = await _categoryAPIClient.UpdateCategory(request);
            return RedirectToAction(nameof(Index), res.IsSuccesss ? false : true);
        }
    }
}