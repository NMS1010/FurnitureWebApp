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
using System.IO;
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
            try
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
                await _context.SaveChangesAsync();

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
                if (request.SubImages != null)
                {
                    ProductImageCreateRequest imageReq = new ProductImageCreateRequest()
                    {
                        ProductId = product.ProductId,
                        Images = request.SubImages
                    };
                    await _productImageService.Create(imageReq);
                }
                return product.ProductId;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<int> Delete(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.ProductId == productId)
                    .Include(c => c.ProductImages)
                    .FirstOrDefaultAsync();

                if (product == null)
                    return -1;
                product.Status = PRODUCT_STATUS.SUSPENDED;
                _context.Products.Update(product);

                return await _context.SaveChangesAsync();
            }
            catch { return -1; }
        }

        private static string GenerateProductStatusClass(int status)
        {
            string s = "";
            switch (status)
            {
                case PRODUCT_STATUS.IN_STOCK:
                    s = "badge badge-success";
                    break;

                case PRODUCT_STATUS.OUT_STOCK:
                    s = "badge badge-warning";
                    break;

                case PRODUCT_STATUS.SUSPENDED:
                    s = "badge badge-danger";
                    break;

                default:
                    s = "";
                    break;
            }
            return s;
        }

        public async Task<PagedResult<ProductViewModel>> RetrieveAll(ProductGetPagingRequest request)
        {
            try
            {
                var query = await _context.Products
                    .Include(c => c.ProductImages)
                    .Include(c => c.Brand)
                    .Include(c => c.Category)
                    .Include(c => c.OrderItems)
                    .ToListAsync();
                if (!String.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(request.Keyword)).ToList();
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
                        TotalPurchased = x.OrderItems.Sum(g => g.Quantity),
                        StatusClass = GenerateProductStatusClass(x.Status)
                    }).ToList();

                foreach (var product in data)
                {
                    product.SubImages = await _productImageService.RetrieveAll(new ProductImageGetPagingRequest() { ProductId = product.ProductId });
                }
                return new PagedResult<ProductViewModel>
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

        public async Task<ProductViewModel> RetrieveById(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.ProductId == productId)
                    .Include(c => c.ProductImages)
                    .Include(c => c.Brand)
                    .Include(c => c.Category)
                    .Include(c => c.OrderItems)
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
                    TotalPurchased = product.OrderItems.Sum(g => g.Quantity),
                    StatusClass = GenerateProductStatusClass(product.Status),
                    SubImages = await _productImageService.RetrieveAll(new ProductImageGetPagingRequest() { ProductId = product.ProductId })
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            try
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
                        await _fileStorageService.DeleteFile(Path.GetFileName(productImg.Path));
                    productImg.IsDefault = true;
                    productImg.Path = await _fileStorageService.SaveFile(request.Image);

                    _context.ProductImages.Update(productImg);
                }
                return await _context.SaveChangesAsync();
            }
            catch
            {
                return -1;
            }
        }
    }
}