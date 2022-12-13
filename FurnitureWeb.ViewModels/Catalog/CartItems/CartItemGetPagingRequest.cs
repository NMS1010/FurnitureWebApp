using FurnitureWeb.ViewModels.Common;

namespace FurnitureWeb.ViewModels.Catalog.CartItems
{
    public class CartItemGetPagingRequest : PagingRequest
    {
        public string UserId { get; set; }
    }
}