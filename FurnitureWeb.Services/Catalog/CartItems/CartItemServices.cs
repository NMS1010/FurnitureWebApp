using Domain.EF;
using Domain.Entities;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.CartItems
{
    public class CartItemServices : ICartItemServices
    {
        private readonly AppDbContext _context;

        public CartItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(CartItemCreateRequest request)
        {
            var cartItem = new CartItem()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                UnitPrice = request.UnitPrice,
                Quantity = request.Quantity,
                TotalPrice = request.UnitPrice * request.Quantity,
                DateAdded = DateTime.Now,
            };

            _context.CartItems.Add(cartItem);

            await _context.SaveChangesAsync();
            return cartItem.CartItemId;
        }

        public async Task<int> Delete(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if (cartItem == null)
                return -1;
            _context.CartItems.Remove(cartItem);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<CartItemViewModel>> RetrieveAll(CartItemGetPagingRequest request)
        {
            var query = await _context.CartItems
                .Include(x => x.User)
                .Include(x => x.Product)
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
                .Select(x => new CartItemViewModel()
                {
                    CartItemId = x.CartItemId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                    DateAdded = DateTime.Now
                }).ToList();

            return new PagedResult<CartItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<CartItemViewModel> RetrieveById(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(p => p.CartItemId == cartItemId)
                .FirstOrDefaultAsync();
            if (cartItem == null)
                return null;
            return new CartItemViewModel()
            {
                CartItemId = cartItem.CartItemId,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                UserId = cartItem.UserId,
                UserName = cartItem.User.UserName,
                UnitPrice = cartItem.UnitPrice,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.TotalPrice,
                DateAdded = DateTime.Now
            };
        }

        public async Task<int> Update(CartItemUpdateRequest request)
        {
            var cartItem = await _context.CartItems
                .Where(c => c.CartItemId == request.CartItemId)
                .FirstOrDefaultAsync();
            if (cartItem == null)
                return -1;
            cartItem.Quantity = request.Quantity;
            cartItem.TotalPrice = request.Quantity * cartItem.UnitPrice;
            return await _context.SaveChangesAsync();
        }
    }
}