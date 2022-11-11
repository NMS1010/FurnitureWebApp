using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Utilities.Constants.Discounts;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Discounts
{
    public class DiscountServices : IDiscountServices
    {
        private readonly AppDbContext _context;

        public DiscountServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> ApllyDiscount(string discountCode)
        {
            var discount = await _context.Discounts
                .Where(p => p.DiscountCode == discountCode)
                .FirstOrDefaultAsync();

            if (discount == null)
                return "error";
            if (discount.Quantity <= 0)
                return "out";
            if (discount.StartDate > DateTime.Now || discount.EndDate < DateTime.Now)
                return "expired";
            discount.Quantity -= 1;
            if (discount.Quantity <= 0)
            {
                discount.Status = DISCOUNT_STATUS.IN_ACTIVE;
                discount.Quantity = 0;
            }
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
            return Newtonsoft.Json.JsonConvert.SerializeObject(discount);
        }

        public async Task<int> Create(DiscountCreateRequest request)
        {
            var discount = new Discount()
            {
                DiscountCode = request.DiscountCode,
                DiscountValue = request.DiscountValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = request.Status,
                Quantity = request.Quantity,
            };

            _context.Discounts.Add(discount);

            await _context.SaveChangesAsync();
            return discount.DiscountId;
        }

        public async Task<int> Delete(int discountId)
        {
            var discount = await _context.Discounts.FindAsync(discountId);

            if (discount == null)
                return -1;
            _context.Discounts.Remove(discount);
            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<DiscountViewModel>> RetrieveAll(DiscountGetPagingRequest request)
        {
            var query = await _context.Discounts
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.DiscountCode.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DiscountViewModel()
                {
                    DiscountId = x.DiscountId,
                    DiscountCode = x.DiscountCode,
                    DiscountValue = x.DiscountValue,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    Quantity = x.Quantity,
                    StatusCode = DISCOUNT_STATUS.DiscountStatus[x.Status]
                }).ToList();

            return new PagedResult<DiscountViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<DiscountViewModel> RetrieveById(int discountId)
        {
            var discount = await _context.Discounts
                .Where(p => p.DiscountId == discountId)
                .FirstOrDefaultAsync();
            if (discount == null)
                return null;
            return new DiscountViewModel()
            {
                DiscountId = discount.DiscountId,
                DiscountCode = discount.DiscountCode,
                DiscountValue = discount.DiscountValue,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                Status = discount.Status,
                Quantity = discount.Quantity,
                StatusCode = DISCOUNT_STATUS.DiscountStatus[discount.Status]
            };
        }

        public async Task<int> Update(DiscountUpdateRequest request)
        {
            var discount = await _context.Discounts
                .Where(c => c.DiscountId == request.DiscountId)
                .FirstOrDefaultAsync();
            if (discount == null)
                return -1;
            discount.DiscountCode = request.DiscountCode;
            discount.DiscountValue = request.DiscountValue;
            discount.StartDate = request.StartDate;
            discount.EndDate = request.EndDate;
            discount.Quantity = request.Quantity;
            if (request.Quantity == 0)
                discount.Status = DISCOUNT_STATUS.IN_ACTIVE;
            else
                discount.Status = request.Status;
            return await _context.SaveChangesAsync();
        }
    }
}