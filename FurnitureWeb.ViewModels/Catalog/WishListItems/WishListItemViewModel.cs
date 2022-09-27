using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.WishListItems
{
    public class WishListItemViewModel
    {
        public int WishListItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime DateAdded { get; set; }
    }
}