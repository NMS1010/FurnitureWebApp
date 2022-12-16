using FurnitureWeb.ViewModels.Common;

namespace FurnitureWeb.ViewModels.Catalog.Orders
{
    public class OrderGetPagingRequest : PagingRequest
    {
        public string UserId { get; set; } = null;
    }
}