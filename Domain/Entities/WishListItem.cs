using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WishListItem
    {
        public int WishListItemId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
        public DateTime DateAdded { get; set; }

        public Product Product { get; set; }
        public AppUser User { get; set; }
    }
}