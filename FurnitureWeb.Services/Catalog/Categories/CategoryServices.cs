using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Categories
{
    public class CategoryServices : ICategoryServices
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public CategoryServices(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<int> Create(CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                Content = request.Content,
                ParentCategoryId = request.ParentCategoryId ?? null,
                Image = await _fileStorageService.SaveFile(request.Image),
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

        public List<CategoryViewModel> GetSubCategory(int categoryId)
        {
            var subCategories = _context.Categories
                .Include(x => x.Products)
                .ThenInclude(x => x.OrderItems)
                .Where(x => x.ParentCategoryId == categoryId).ToList();
            List<CategoryViewModel> res = new List<CategoryViewModel>();
            if (subCategories.Count == 0)
                return res;
            subCategories.ForEach(x =>
            {
                res.Add(new CategoryViewModel()
                {
                    CategoryId = x.CategoryId,
                    Content = x.Content,
                    ParentCategoryId = x.ParentCategoryId,
                    Image = x.Image,
                    Name = x.Name,
                    ParentCategoryName = _context.Categories.Find(x.ParentCategoryId)?.Name,
                    TotalProduct = x.Products.Count,
                    SubCategories = GetSubCategory(x.CategoryId)
                });
            });

            return res;
        }

        public async Task<PagedResult<CategoryViewModel>> RetrieveAll(CategoryGetPagingRequest request)
        {
            var query = await _context.Categories
                .Include(x => x.Products)
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
                    Content = x.Content,
                    Image = x.Image,
                    SubCategories = GetSubCategory(x.CategoryId),
                    TotalProduct = x.Products.Count
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
                .Include(x => x.Products)
                .FirstOrDefaultAsync();
            if (category == null)
                return null;
            return new CategoryViewModel()
            {
                CategoryId = categoryId,
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId ?? null,
                ParentCategoryName = _context.Categories.Find(category.ParentCategoryId)?.Name,
                Content = category.Content,
                Image = category.Image,
                SubCategories = GetSubCategory(category.CategoryId),
                TotalProduct = category.Products.Count
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
            await _fileStorageService.DeleteFile(category.Image);
            category.Image = await _fileStorageService.SaveFile(request.Image);
            return await _context.SaveChangesAsync();
        }
    }
}