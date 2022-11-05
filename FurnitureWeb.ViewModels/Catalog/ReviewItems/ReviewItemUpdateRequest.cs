using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.ReviewItems
{
    public class ReviewItemUpdateRequest
    {
        [Required]
        public int ReviewItemId { get; set; }

        [MaxLength(255)]
        public string Content { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public int Status { get; set; }
    }
}