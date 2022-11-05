using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ProductImages
{
    public class ProductImageServices : IProductImageServices
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public ProductImageServices(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<int> Create(ProductImageCreateRequest request)
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
                    Path = await _fileStorageService.SaveFile(item)
                });
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);
            if (productImage == null)
                return -1;
            _context.ProductImages.Remove(productImage);
            await _fileStorageService.DeleteFile(productImage.Path);
            return await _context.SaveChangesAsync();
        }

        public async Task<ProductImage> RetrieveById(int productImageId)
        {
            var productImage = await _context.ProductImages.FindAsync(productImageId);
            if (productImage == null)
                return null;
            return productImage;
        }

        public async Task<int> Update(ProductImageUpdateRequest request)
        {
            var productImg = await _context.ProductImages.FindAsync(request.ProductImageId);
            if (productImg == null)
                return -1;
            if (request.Image == null)
                return -1;

            await _fileStorageService.DeleteFile(productImg.Path);
            productImg.IsDefault = false;
            productImg.Path = await _fileStorageService.SaveFile(request.Image);

            _context.ProductImages.Update(productImg);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductImage>> RetrieveAll(ProductImageGetPagingRequest request)
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
    }
}