using Microsoft.AspNetCore.Http;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductImageUpdateRequest
    {
        public int ProductId { get; set; }
        public int ProductImageId { get; set; }
        public IFormFile Image { get; set; }
    }
}