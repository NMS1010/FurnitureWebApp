using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.ProductImages
{
    public class ProductImageCreateSingleRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}