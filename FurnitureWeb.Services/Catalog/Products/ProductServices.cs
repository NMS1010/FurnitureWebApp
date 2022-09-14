using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
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

        public ProductServices(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<int> AddImages(ProductImageCreateRequest request)
        {
            var product = await _context.Products
                .Where(c => c.ProductId == request.ProductId)
                .Include(c => c.ProductImages)
                .FirstOrDefaultAsync();

            if (product == null)
                return -1;

            foreach (var item in request.Images)
            {
                _context.ProductImages.Add(new ProductImage()
                {
                    ProductId = product.ProductId,
                    IsDefault = false,
                    Size = item.Length,
                    Path = await _fileStorageService.SaveFile(item)
                });
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Quantity = request.Quantity,
                DateCreated = request.DateCreated,
                Origin = request.Origin,
                Status = request.Status,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            if (request.Image != null)
            {
                _context.ProductImages.Add(new ProductImage()
                {
                    IsDefault = true,
                    ProductId = product.ProductId,
                    Size = request.Image.Length,
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
            _context.Products.Remove(product);
            foreach (var item in product.ProductImages)
            {
                await _fileStorageService.DeleteFile(item.Path);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteImage(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);
            if (productImage == null)
                return -1;
            _context.ProductImages.Remove(productImage);
            await _fileStorageService.DeleteFile(productImage.Path);
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
                        ?.Path
                }).ToList();

            return new PagedResult<ProductViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<ProductViewModel> RetrieveByProductId(int productId)
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
                        .Path
            };
        }

        public async Task<ProductImage> RetrieveImageById(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);

            return productImage;
        }

        public async Task<PagedResult<ProductImage>> RetrieveImages(ProductImageGetPagingRequest request)
        {
            var query = await _context.ProductImages
                .Where(c => c.ProductId == request.ProductId)
                .ToListAsync();

            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResult<ProductImage>
            {
                TotalItem = query.Count,
                Items = data
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
                productImg.Size = request.Image.Length;

                _context.ProductImages.Update(productImg);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(ProductImageUpdateRequest request)
        {
            var productImg = await _context.ProductImages.FindAsync(request.ProductImageId);
            if (productImg == null)
                return -1;
            if (request.Image == null)
                return -1;

            await _fileStorageService.DeleteFile(productImg.Path);
            productImg.IsDefault = false;
            productImg.Path = await _fileStorageService.SaveFile(request.Image);
            productImg.Size = request.Image.Length;

            _context.ProductImages.Update(productImg);

            return await _context.SaveChangesAsync();
        }
    }
}