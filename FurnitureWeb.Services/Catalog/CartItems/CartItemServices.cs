using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Utilities.Constants.Products;
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
                Quantity = request.Quantity,
                DateAdded = DateTime.Now,
                Status = request.Status,
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
                .Select(x => new CartItemViewModel()
                {
                    CartItemId = x.CartItemId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ImageProduct = x.Product.ProductImages
                        .Where(c => c.ProductId == x.ProductId && c.IsDefault == true)
                        .FirstOrDefault()?.Path,
                    Quantity = x.Quantity,
                    TotalPrice = x.Quantity * x.Product.Price,
                    UnitPrice = x.Product.Price,
                    DateAdded = DateTime.Now,
                    Status = x.Status,
                    ProductStatus = PRODUCT_STATUS.ProductStatus[x.Status]
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
                .ThenInclude(x => x.ProductImages)
                .Where(p => p.CartItemId == cartItemId)
                .FirstOrDefaultAsync();
            if (cartItem == null)
                return null;
            return new CartItemViewModel()
            {
                CartItemId = cartItem.CartItemId,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                ImageProduct = cartItem.Product.ProductImages
                    .Where(x => x.ProductId == cartItem.ProductId && x.IsDefault == true)
                    .FirstOrDefault()?.Path,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Quantity * cartItem.Product.Price,
                UnitPrice = cartItem.Product.Price,
                DateAdded = DateTime.Now,
                Status = cartItem.Status,
                ProductStatus = PRODUCT_STATUS.ProductStatus[cartItem.Status]
            };
        }

        public async Task<int> Update(CartItemUpdateRequest request)
        {
            var cartItem = await _context.CartItems
                .Include(x => x.Product)
                .Where(c => c.CartItemId == request.CartItemId)
                .FirstOrDefaultAsync();
            if (cartItem == null)
                return -1;
            cartItem.Quantity = request.Quantity;

            cartItem.Status = request.Status;
            return await _context.SaveChangesAsync();
        }
    }
}