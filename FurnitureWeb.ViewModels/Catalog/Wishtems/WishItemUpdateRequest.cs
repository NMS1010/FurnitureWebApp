using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Catalog.Wishtems
{
    public class WishItemUpdateRequest
    {
        public int WishItemId { get; set; }
        public int Status { get; set; }
    }
}