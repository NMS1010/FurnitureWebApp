using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.Products
{
    public class ProductImageCreateRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }
    }
}