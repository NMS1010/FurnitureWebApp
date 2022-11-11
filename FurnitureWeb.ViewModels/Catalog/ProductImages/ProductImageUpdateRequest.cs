using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FurnitureWeb.ViewModels.Catalog.ProductImages
{
    public class ProductImageUpdateRequest
    {
        [Required]
        public int ProductImageId;

        [Required]
        public IFormFile Image;
    }
}