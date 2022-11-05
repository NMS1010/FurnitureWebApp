using Domain.EF;
using Domain.Entities;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ReviewItems
{
    public class ReviewItemServices : IReviewItemServices
    {
        private readonly AppDbContext _context;

        public ReviewItemServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ReviewItemCreateRequest request)
        {
            var review = new ReviewItem()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Content = request.Content,
                Rating = request.Rating,
                Status = request.Status,
                DateCreated = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review.ReviewItemId;
        }

        public async Task<int> Delete(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return -1;
            _context.Reviews.Remove(review);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ReviewItemViewModel>> RetrieveAll(ReviewItemGetPagingRequest request)
        {
            var query = await _context.Reviews
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query
                    .Where(x => x.Content.Contains(request.Keyword))
                    .ToList();
            }
            var data = query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ReviewItemViewModel()
                {
                    ReviewItemId = x.ReviewItemId,
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductImage = x.Product.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == x.ProductId)
                        .FirstOrDefault()?.Path,
                    Rating = x.Rating,
                    Content = x.Content,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Status = x.Status,
                    UserName = x.User.UserName
                }).ToList();

            return new PagedResult<ReviewItemViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<ReviewItemViewModel> RetrieveById(int reviewId)
        {
            var review = await _context.Reviews
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Where(x => x.ReviewItemId == reviewId)
                .FirstOrDefaultAsync();
            if (review == null)
                return null;
            return new ReviewItemViewModel()
            {
                ReviewItemId = review.ReviewItemId,
                ProductId = review.ProductId,
                ProductName = review.Product.Name,
                ProductImage = review.Product.ProductImages
                    .Where(c => c.IsDefault == true && c.ProductId == review.ProductId)
                    .FirstOrDefault()?.Path,
                UserId = review.UserId,
                Content = review.Content,
                Rating = review.Rating,
                DateCreated = review.DateCreated,
                DateUpdated = review.DateUpdated,
                Status = review.Status,
                UserName = review.User.UserName
            };
        }

        public async Task<int> Update(ReviewItemUpdateRequest request)
        {
            var review = await _context.Reviews.FindAsync(request.ReviewItemId);
            if (review == null)
                return -1;
            review.Content = request.Content;
            review.Rating = request.Rating;
            review.DateUpdated = DateTime.Now;
            review.Status = request.Status;
            _context.Reviews.Update(review);

            return await _context.SaveChangesAsync();
        }
    }
}