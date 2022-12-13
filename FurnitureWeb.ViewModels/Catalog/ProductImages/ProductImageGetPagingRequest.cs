using FurnitureWeb.ViewModels.Common;

namespace FurnitureWeb.ViewModels.Catalog.ProductImages
{
    public class ProductImageGetPagingRequest : PagingRequest
    {
        public int ProductId { get; set; }
    }
}