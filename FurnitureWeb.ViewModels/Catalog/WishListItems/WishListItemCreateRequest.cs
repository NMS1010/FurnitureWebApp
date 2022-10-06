using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.WishListItems
{
    public class WishListItemCreateRequest
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public int Status { get; set; }
    }
}