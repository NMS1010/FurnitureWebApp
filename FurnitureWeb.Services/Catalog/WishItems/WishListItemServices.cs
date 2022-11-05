using Domain.EF;
using Domain.Entities;
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
    public class WishListItemServices : IWishListItemServices
    {
        private readonly AppDbContext _context;

        public WishListItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(WishItemCreateRequest request)
        {
            var wishListItem = new WishItem()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                DateAdded = DateTime.Now,
                Status = request.Status,
            };

            _context.WishLists.Add(wishListItem);
            await _context.SaveChangesAsync();

            return wishListItem.WishItemId;
        }

        public async Task<int> Delete(int wishListItemId)
        {
            var wishListItem = await _context.WishLists.FindAsync(wishListItemId);
            if (wishListItem == null)
                return -1;
            _context.WishLists.Remove(wishListItem);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<WishItemViewModel>> RetrieveAll(WishItemGetPagingRequest request)
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
                    UserName = x.User.UserName
                }).ToList();

            return new PagedResult<WishItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<WishItemViewModel> RetrieveById(int wishListItemId)
        {
            var wishListItem = await _context.WishLists
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Where(x => x.WishItemId == wishListItemId)
                .FirstOrDefaultAsync();
            if (wishListItem == null)
                return null;
            return new WishItemViewModel()
            {
                WishItemId = wishListItem.WishItemId,
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

        public async Task<int> Update(WishItemUpdateRequest request)
        {
            var wishListItem = await _context.WishLists.FindAsync(request.WishItemId);
            if (wishListItem == null)
                return -1;
            _context.WishLists.Update(wishListItem);
            wishListItem.Status = request.Status;
            return await _context.SaveChangesAsync();
        }
    }
}