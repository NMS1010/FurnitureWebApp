using Domain.EF;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Categories
{
    public class CategoryServices : ICategoryServices
    {
        private readonly AppDbContext _context;

        public CategoryServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Domain.Entities.Category()
            {
                Name = request.Name,
                Content = request.Content,
                ParentCategoryId = request.ParentCategoryId ?? null,
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();
            return category.CategoryId;
        }

        public async Task<int> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category == null)
                return -1;
            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<CategoryViewModel>> RetrieveAll(CategoryGetPagingRequest request)
        {
            var query = await _context.Categories
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.Name.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CategoryViewModel()
                {
                    CategoryId = x.CategoryId,
                    Name = x.Name,
                    ParentCategoryId = x.ParentCategoryId ?? null,
                    ParentCategoryName = _context.Categories.Find(x.ParentCategoryId)?.Name,
                    Content = x.Content
                }).ToList();

            return new PagedResult<CategoryViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<CategoryViewModel> RetrieveById(int categoryId)
        {
            var category = await _context.Categories
                .Where(p => p.CategoryId == categoryId)
                .FirstOrDefaultAsync();
            if (category == null)
                return null;
            return new CategoryViewModel()
            {
                CategoryId = categoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId ?? null,
                ParentCategoryName = _context.Categories.Find(category.ParentCategoryId)?.Name,
                Content = category.Content
            };
        }

        public async Task<int> Update(CategoryUpdateRequest request)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == request.CategoryId)
                .FirstOrDefaultAsync();
            if (category == null)
                return -1;
            category.Name = request.Name;
            category.ParentCategoryId = request.ParentCategoryId ?? null;
            category.Content = request.Content;
            return await _context.SaveChangesAsync();
        }
    }
}