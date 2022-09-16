using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.OrderItemItems
{
    public class OrderItemServices : IOrderItemServices
    {
        private readonly AppDbContext _context;

        public OrderItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(OrderItemCreateRequest request)
        {
            var orderItem = new OrderItem()
            {
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return orderItem.OrderItemId;
        }

        public async Task<int> Delete(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
                return -1;
            _context.OrderItems.Remove(orderItem);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<OrderItemViewModel>> RetrieveAll(OrderItemGetPagingRequest request)
        {
            var query = await _context.OrderItems
                .Include(x => x.Product)
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderItemViewModel()
                {
                    OrderItemId = x.OrderItemId,
                    OrderId = x.OrderId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    TotalPrice = x.TotalPrice
                }).ToList();

            return new PagedResult<OrderItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<OrderItemViewModel> RetrieveById(int orderItemId)
        {
            var orderItem = await _context.OrderItems
                .Include(x => x.Product)
                .Where(x => x.OrderItemId == orderItemId)
                .FirstOrDefaultAsync();
            if (orderItem == null)
                return null;
            return new OrderItemViewModel()
            {
                OrderItemId = orderItem.OrderItemId,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductName = orderItem.Product.Name,
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice,
                TotalPrice = orderItem.TotalPrice
            };
        }

        public async Task<int> Update(OrderItemUpdateRequest request)
        {
            var orderItem = await _context.OrderItems.FindAsync(request.OrderItemId);
            if (orderItem == null)
                return -1;

            _context.OrderItems.Update(orderItem);

            return await _context.SaveChangesAsync();
        }
    }
}