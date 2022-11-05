using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        [Required]
        public Dictionary<int, IFormFile> ProductImages { get; set; }
    }
}