﻿using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
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
            try
            {
                var brand = new Brand()
                {
                    BrandName = request.BrandName,
                    Origin = request.Origin,
                    Image = await _fileStorage.SaveFile(request.Image),
                };
                _context.Brands.Add(brand);
                await _context.SaveChangesAsync();
                return brand.BrandId;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> Delete(int brandId)
        {
            try
            {
                var brand = await _context.Brands.FindAsync(brandId);
                if (brand == null)
                    return -1;
                _context.Brands.Remove(brand);
                int count = await _context.SaveChangesAsync();
                await _fileStorage.DeleteFile(Path.GetFileName(brand.Image));
                return count;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<PagedResult<BrandViewModel>> RetrieveAll(BrandGetPagingRequest request)
        {
            try
            {
                var query = await _context.Brands
                    .Include(x => x.Products)
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
                        Image = x.Image,
                        TotalProduct = x.Products.Count
                    }).ToList();

                return new PagedResult<BrandViewModel>
                {
                    TotalItem = query.Count,
                    Items = data
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<BrandViewModel> RetrieveById(int brandId)
        {
            try
            {
                var brand = await _context.Brands.Include(x => x.Products).Where(x => x.BrandId == brandId).FirstOrDefaultAsync();
                if (brand == null)
                    return null;
                return new BrandViewModel()
                {
                    BrandId = brand.BrandId,
                    BrandName = brand.BrandName,
                    Origin = brand.Origin,
                    Image = brand.Image,
                    TotalProduct = brand.Products.Count
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> Update(BrandUpdateRequest request)
        {
            try
            {
                var brand = await _context.Brands.FindAsync(request.BrandId);
                if (brand == null)
                    return -1;
                brand.BrandName = request.BrandName;
                brand.Origin = request.Origin;
                string filename = brand.Image;
                if (request.Image != null)
                {
                    brand.Image = await _fileStorage.SaveFile(request.Image);
                }
                _context.Brands.Update(brand);

                int count = await _context.SaveChangesAsync();
                await _fileStorage.DeleteFile(Path.GetFileName(filename));
                return count;
            }
            catch
            {
                return -1;
            }
        }
    }
}