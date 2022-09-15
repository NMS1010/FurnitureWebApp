﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Reviews
{
    public class ReviewCreateRequest
    {
        public int? ParentReviewId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        public double? Rating { get; set; }
    }
}