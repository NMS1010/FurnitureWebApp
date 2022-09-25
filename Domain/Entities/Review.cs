using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public int? Rating { get; set; }
        public string Content { get; set; }

        public Product Product { get; set; }
        public AppUser User { get; set; }
    }
}