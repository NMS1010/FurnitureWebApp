﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Brands
{
    public class BrandUpdateRequest
    {
        [Required]
        public int BrandId { get; set; }

        [Required]
        [MaxLength(255)]
        public string BrandName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Origin { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}