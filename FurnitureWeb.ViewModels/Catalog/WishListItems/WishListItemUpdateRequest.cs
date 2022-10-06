using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.WishListItems
{
    public class WishListItemUpdateRequest
    {
        public int WishListItemId { get; set; }
        public int Status { get; set; }
    }
}