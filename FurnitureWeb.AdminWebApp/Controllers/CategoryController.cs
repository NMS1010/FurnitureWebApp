using FurnitureWeb.APICaller.Category;
using FurnitureWeb.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
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

        [Route("sub")]
        public async Task<IActionResult> SubCategory(bool error = false)
        {
            var res = await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest());

            if (error || !res.IsSuccesss)
                ViewData["Error"] = res.Errors;
            ViewData["parentCategories"] = res.Data.Items.Where(x => x.ParentCategoryId == null).ToList();
            res.Data.Items.RemoveAll(x => x.ParentCategoryId == null);
            return View(res.Data);
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            if (error)
                ViewData["Error"] = error;
            var res = await _categoryAPIClient.GetAllCategoryAsync(new CategoryGetPagingRequest());
            res.Data.Items.RemoveAll(x => x.ParentCategoryId != null);
            return View(res.Data);
        }

        [HttpPost("add/{sub?}")]
        public async Task<IActionResult> Create(CategoryCreateRequest request, string sub = null)
        {
            var res = await _categoryAPIClient.CreateCategory(request);
            string action = sub == null ? nameof(Index) : nameof(SubCategory);
            return RedirectToAction(action, new { error = !res.IsSuccesss });
        }

        [HttpGet("delete/{categoryId}/{sub?}")]
        public async Task<IActionResult> Delete(int categoryId, string sub = null)
        {
            var res = await _categoryAPIClient.DeleteCategory(categoryId);

            string action = sub == null ? nameof(Index) : nameof(SubCategory);
            return RedirectToAction(action, new { error = !res.IsSuccesss });
        }

        [Route("get/{categoryId}")]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var res = await _categoryAPIClient.GetCategoryById(categoryId);
            if (res == null || !res.IsSuccesss)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit/{sub?}")]
        public async Task<IActionResult> Edit(CategoryUpdateRequest request, string sub = null)
        {
            var res = await _categoryAPIClient.UpdateCategory(request);
            string action = sub == null ? nameof(Index) : nameof(SubCategory);
            return RedirectToAction(action, new { error = !res.IsSuccesss });
        }
    }
}