using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Brands
{
    public class BrandServices : IBrandServices
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorage;

        public BrandServices(AppDbContext context, IFileStorageService fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<int> Create(BrandCreateRequest request)
        {
            var brand = new Brand()
            {
                BrandName = request.BrandName,
                Origin = request.Origin,
                ImagePath = await _fileStorage.SaveFile(request.Image),
                ImageSize = request.Image.Length
            };
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return brand.BrandId;
        }

        public async Task<int> Delete(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
                return -1;
            await _fileStorage.DeleteFile(brand.ImagePath);
            _context.Brands.Remove(brand);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<BrandViewModel>> RetrieveAll(BrandGetPagingRequest request)
        {
            var query = await _context.Brands
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.BrandName.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BrandViewModel()
                {
                    BrandId = x.BrandId,
                    BrandName = x.BrandName,
                    Origin = x.Origin,
                    ImagePath = x.ImagePath,
                    ImageSize = x.ImageSize,
                }).ToList();

            return new PagedResult<BrandViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<BrandViewModel> RetrieveById(int brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            if (brand == null)
                return null;
            return new BrandViewModel()
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                Origin = brand.Origin,
                ImagePath = brand.ImagePath,
                ImageSize = brand.ImageSize,
            };
        }

        public async Task<int> Update(BrandUpdateRequest request)
        {
            var brand = await _context.Brands.FindAsync(request.BrandId);
            if (brand == null)
                return -1;
            brand.BrandName = request.BrandName;
            brand.Origin = request.Origin;
            if (request.Image != null)
            {
                await _fileStorage.DeleteFile(brand.ImagePath);
                brand.ImagePath = await _fileStorage.SaveFile(request.Image);
                brand.ImageSize = request.Image.Length;
            }
            _context.Brands.Update(brand);

            return await _context.SaveChangesAsync();
        }
    }
}