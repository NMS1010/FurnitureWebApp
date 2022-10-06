using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Catalog.WishListItems;
using FurnitureWeb.ViewModels.Catalog.WishListItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.WishListItems
{
    public class WishListItemServices : IWishListItemServices
    {
        private readonly AppDbContext _context;

        public WishListItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(WishListItemCreateRequest request)
        {
            var wishListItem = new WishListItem()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                DateAdded = DateTime.Now,
                Status = request.Status,
            };

            _context.WishLists.Add(wishListItem);
            await _context.SaveChangesAsync();

            return wishListItem.WishListItemId;
        }

        public async Task<int> Delete(int wishListItemId)
        {
            var wishListItem = await _context.WishLists.FindAsync(wishListItemId);
            if (wishListItem == null)
                return -1;
            _context.WishLists.Remove(wishListItem);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<WishListItemViewModel>> RetrieveAll(WishListItemGetPagingRequest request)
        {
            var query = await _context.WishLists
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
                .Select(x => new WishListItemViewModel()
                {
                    WishListItemId = x.WishListItemId,
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductImage = x.Product.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == x.ProductId)
                        .FirstOrDefault()?.Path,
                    DateAdded = x.DateAdded,
                    Status = x.Status,
                    UnitPrice = x.Product.Price,
                    UserName = x.User.UserName
                }).ToList();

            return new PagedResult<WishListItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<WishListItemViewModel> RetrieveById(int wishListItemId)
        {
            var wishListItem = await _context.WishLists
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Where(x => x.WishListItemId == wishListItemId)
                .FirstOrDefaultAsync();
            if (wishListItem == null)
                return null;
            return new WishListItemViewModel()
            {
                WishListItemId = wishListItem.WishListItemId,
                ProductId = wishListItem.ProductId,
                ProductName = wishListItem.Product.Name,
                ProductImage = wishListItem.Product.ProductImages
                    .Where(c => c.IsDefault == true && c.ProductId == wishListItem.ProductId)
                    .FirstOrDefault()?.Path,
                UserId = wishListItem.UserId,
                DateAdded = wishListItem.DateAdded,
                Status = wishListItem.Status,
                UnitPrice = wishListItem.Product.Price,
                UserName = wishListItem.User.UserName
            };
        }

        public async Task<int> Update(WishListItemUpdateRequest request)
        {
            var wishListItem = await _context.WishLists.FindAsync(request.WishListItemId);
            if (wishListItem == null)
                return -1;
            _context.WishLists.Update(wishListItem);
            wishListItem.Status = request.Status;
            return await _context.SaveChangesAsync();
        }
    }
}