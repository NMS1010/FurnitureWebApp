using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Reviews
{
    public class ReviewUpdateRequest
    {
        [Required]
        public int ReviewId { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        public int? Rating { get; set; }

        public int Status { get; set; }
    }
}