using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Utilities.Constants.Products;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.WishItems
{
    public class WishItemServices : IWishItemServices
    {
        private readonly AppDbContext _context;

        public WishItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(WishItemCreateRequest request)
        {
            var wishItem = new WishItem()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                DateAdded = DateTime.Now,
                Status = request.Status,
            };

            _context.WishItems.Add(wishItem);
            await _context.SaveChangesAsync();

            return wishItem.WishItemId;
        }

        public async Task<int> Delete(int wishItemId)
        {
            var wishItem = await _context.WishItems.FindAsync(wishItemId);
            if (wishItem == null)
                return -1;
            _context.WishItems.Remove(wishItem);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<WishItemViewModel>> RetrieveAll(WishItemGetPagingRequest request)
        {
            var query = await _context.WishItems
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.Product.Name.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WishItemViewModel()
                {
                    WishItemId = x.WishItemId,
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductImage = x.Product.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == x.ProductId)
                        .FirstOrDefault()?.Path,
                    DateAdded = x.DateAdded,
                    Status = x.Status,
                    UnitPrice = x.Product.Price,
                    UserName = x.User.UserName,
                    ProductStatus = PRODUCT_STATUS.ProductStatus[x.Status]
                }).ToList();

            return new PagedResult<WishItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<WishItemViewModel> RetrieveById(int wishItemId)
        {
            var wishItem = await _context.WishItems
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Where(x => x.WishItemId == wishItemId)
                .FirstOrDefaultAsync();
            if (wishItem == null)
                return null;
            return new WishItemViewModel()
            {
                WishItemId = wishItem.WishItemId,
                ProductId = wishItem.ProductId,
                ProductName = wishItem.Product.Name,
                ProductImage = wishItem.Product.ProductImages
                    .Where(c => c.IsDefault == true && c.ProductId == wishItem.ProductId)
                    .FirstOrDefault()?.Path,
                UserId = wishItem.UserId,
                DateAdded = wishItem.DateAdded,
                Status = wishItem.Status,
                UnitPrice = wishItem.Product.Price,
                UserName = wishItem.User.UserName,
                ProductStatus = PRODUCT_STATUS.ProductStatus[wishItem.Status]
            };
        }

        public async Task<int> Update(WishItemUpdateRequest request)
        {
            var wishItem = await _context.WishItems.FindAsync(request.WishItemId);
            if (wishItem == null)
                return -1;
            _context.WishItems.Update(wishItem);
            wishItem.Status = request.Status;
            return await _context.SaveChangesAsync();
        }
    }
}