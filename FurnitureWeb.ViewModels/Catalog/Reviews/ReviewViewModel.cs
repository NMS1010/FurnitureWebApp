using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Reviews
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; }
        public int Status { get; set; }
    }
}