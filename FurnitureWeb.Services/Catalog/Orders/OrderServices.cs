using Domain.EF;
using Domain.Entities;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Orders
{
    public class OrderServices : IOrderServices
    {
        private readonly AppDbContext _context;

        public OrderServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(OrderCreateRequest request)
        {
            var order = new Order()
            {
                UserId = request.UserId,
                DiscountId = request.DiscountId,
                Shipping = request.Shipping,
                TotalItemPrice = request.TotalItemPrice,
                TotalPrice = request.Shipping + request.TotalItemPrice,
                Address = request.Address,
                DateCreated = DateTime.Now,
                Status = request.Status,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
            };
            if (request.Status == 1)
            {
                order.DateDone = DateTime.Now;
            }
            else
            {
                order.DateDone = null;
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.OrderId;
        }

        public async Task<int> Delete(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return -1;
            _context.Orders.Remove(order);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<OrderViewModel>> RetrieveAll(OrderGetPagingRequest request)
        {
            var query = await _context.Orders
                .Include(x => x.User)
                .Include(x => x.Discount)
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.Address.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OrderViewModel()
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    UserName = x.User.UserName,
                    DiscountId = x.DiscountId,
                    DiscountCode = x.Discount.DiscountCode,
                    DiscountValue = x.Discount.DiscountValue,
                    Shipping = x.Shipping,
                    TotalItemPrice = x.TotalItemPrice,
                    TotalPrice = x.TotalPrice,
                    Address = x.Address,
                    DateCreated = x.DateCreated,
                    DateDone = x.DateDone,
                    Status = x.Status,
                    Name = x.Name,
                    Email = x.Email,
                    Phone = x.Phone
                }).ToList();

            return new PagedResult<OrderViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<OrderViewModel> RetrieveById(int orderId)
        {
            var order = await _context.Orders
                .Include(x => x.User)
                .Include(x => x.Discount)
                .Where(x => x.OrderId == orderId)
                .FirstOrDefaultAsync();
            if (order == null)
                return null;
            return new OrderViewModel()
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                UserName = order.User.UserName,
                DiscountId = order.DiscountId,
                DiscountCode = order.Discount.DiscountCode,
                DiscountValue = order.Discount.DiscountValue,
                Shipping = order.Shipping,
                TotalItemPrice = order.TotalItemPrice,
                TotalPrice = order.TotalPrice,
                Address = order.Address,
                DateCreated = order.DateCreated,
                DateDone = order.DateDone,
                Status = order.Status,
                Name = order.Name,
                Email = order.Email,
                Phone = order.Phone
            };
        }

        public async Task<int> Update(OrderUpdateRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
                return -1;
            if (request.Status != 1)
            {
                order.Status = request.Status;
            }

            if (request.Status == 1)
            {
                order.DateDone = DateTime.Now;
            }
            _context.Orders.Update(order);

            return await _context.SaveChangesAsync();
        }
    }
}