using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Catalog.ProductImages;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.Utilities.Constants.Products;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Products
{
    public class ProductServices : IProductServices
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        private readonly IProductImageServices _productImageService;

        public ProductServices(AppDbContext context, IFileStorageService fileStorageService, IProductImageServices productImageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
            _productImageService = productImageService;
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                DateCreated = DateTime.Now,
                Origin = request.Origin,
                Status = request.Status,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId
            };

            _context.Products.Add(product);

            if (request.Image != null)
            {
                _context.ProductImages.Add(new ProductImage()
                {
                    IsDefault = true,
                    ProductId = product.ProductId,
                    Path = await _fileStorageService.SaveFile(request.Image)
                });
            }
            await _context.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == productId)
                .Include(c => c.ProductImages)
                .FirstOrDefaultAsync();

            if (product == null)
                return -1;
            product.Status = PRODUCT_STATUS.SUSPENDED;
            _context.Products.Update(product);
            //foreach (var item in product.ProductImages)
            //{
            //    await _fileStorageService.DeleteFile(item.Path);
            //}
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> RetrieveAll(ProductGetPagingRequest request)
        {
            var query = await _context.Products
                .Include(c => c.ProductImages)
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .ToListAsync();
            if (!String.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword)).ToList();
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(x => request.CategoryIds.Contains(x.CategoryId)).ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductViewModel()
                {
                    ProductId = x.ProductId,
                    Name = x.Name,
                    Description = x.Description,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    DateCreated = x.DateCreated,
                    Status = x.Status,
                    Origin = x.Origin,
                    CategoryName = x.Category.Name,
                    BrandName = x.Brand.BrandName,
                    ImagePath = x.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == x.ProductId)
                        .FirstOrDefault()
                        ?.Path,
                    BrandId = x.BrandId,
                    CategoryId = x.CategoryId,
                    StatusCode = PRODUCT_STATUS.ProductStatus[x.Status],
                    TotalPurchased = x.OrderItems.Sum(g => g.Quantity)
                }).ToList();

            return new PagedResult<ProductViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<ProductViewModel> RetrieveById(int productId)
        {
            var product = await _context.Products
                .Where(p => p.ProductId == productId)
                .Include(c => c.ProductImages)
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .FirstOrDefaultAsync();
            if (product == null)
                return null;
            return new ProductViewModel()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                DateCreated = product.DateCreated,
                Status = product.Status,
                Origin = product.Origin,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.BrandName,
                ImagePath = product.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == product.ProductId)
                        .FirstOrDefault()
                        .Path,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                StatusCode = PRODUCT_STATUS.ProductStatus[product.Status],
                TotalPurchased = product.OrderItems.Sum(g => g.Quantity)
            };
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products
                .Where(c => c.ProductId == request.ProductId)
                .Include(c => c.ProductImages)
                .FirstOrDefaultAsync();
            if (product == null)
                return -1;
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.Status = request.Status;
            product.Origin = request.Origin;
            product.BrandId = request.BrandId;
            product.CategoryId = request.CategoryId;

            if (request.Image != null)
            {
                var productImg = await _context.ProductImages
                    .Where(c => c.IsDefault == true && c.ProductId == request.ProductId)
                    .FirstOrDefaultAsync();
                if (productImg != null)
                    await _fileStorageService.DeleteFile(productImg.Path);
                productImg.IsDefault = true;
                productImg.Path = await _fileStorageService.SaveFile(request.Image);

                _context.ProductImages.Update(productImg);
            }
            return await _context.SaveChangesAsync();
        }
    }
}