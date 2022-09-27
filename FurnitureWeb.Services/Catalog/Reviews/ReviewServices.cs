using Domain.EF;
using Domain.Entities;
using FurnitureWeb.ViewModels.Catalog.Reviews;
using FurnitureWeb.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Reviews
{
    public class ReviewServices : IReviewServices
    {
        private readonly AppDbContext _context;

        public ReviewServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ReviewCreateRequest request)
        {
            var review = new Review()
            {
                ProductId = request.ProductId,
                UserId = request.UserId,
                Content = request.Content,
                Rating = request.Rating,
                Status = 1,
                DateCreated = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review.ReviewId;
        }

        public async Task<int> Delete(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                return -1;
            _context.Reviews.Remove(review);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ReviewViewModel>> RetrieveAll(ReviewGetPagingRequest request)
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
                .Select(x => new ReviewViewModel()
                {
                    ReviewId = x.ReviewId,
                    UserId = x.UserId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    ProductImage = x.Product.ProductImages
                        .Where(c => c.IsDefault == true && c.ProductId == x.ProductId)
                        .FirstOrDefault()?.Path,
                    Rating = x.Rating,
                    Content = x.Content,
                    DateCreated = x.DateCreated,
                    Status = x.Status,
                    UserName = x.User.UserName
                }).ToList();

            return new PagedResult<ReviewViewModel>
            {
                TotalItem = query.Count,
                Items = data
            };
        }

        public async Task<ReviewViewModel> RetrieveById(int reviewId)
        {
            var review = await _context.Reviews
                .Include(x => x.User)
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Where(x => x.ReviewId == reviewId)
                .FirstOrDefaultAsync();
            if (review == null)
                return null;
            return new ReviewViewModel()
            {
                ReviewId = review.ReviewId,
                ProductId = review.ProductId,
                ProductName = review.Product.Name,
                ProductImage = review.Product.ProductImages
                    .Where(c => c.IsDefault == true && c.ProductId == review.ProductId)
                    .FirstOrDefault()?.Path,
                UserId = review.UserId,
                Content = review.Content,
                Rating = review.Rating,
                DateCreated = review.DateCreated,
                Status = review.Status,
                UserName = review.User.UserName
            };
        }

        public async Task<int> Update(ReviewUpdateRequest request)
        {
            var review = await _context.Reviews.FindAsync(request.ReviewId);
            if (review == null)
                return -1;
            review.Content = request.Content;
            review.Rating = request.Rating;
            review.DateCreated = DateTime.Now;
            review.Status = request.Status;
            _context.Reviews.Update(review);

            return await _context.SaveChangesAsync();
        }
    }
}