using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductImageUpdateRequest
    {
        [Required]
        public int ProductImageId { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}