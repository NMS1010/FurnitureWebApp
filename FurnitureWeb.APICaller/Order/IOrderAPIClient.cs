using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Order
{
    public interface IOrderAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateOrder(OrderUpdateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> CreateOrder(OrderCreateRequest request);

        Task<CustomAPIResponse<PagedResult<OrderViewModel>>> GetAllOrderAsync(OrderGetPagingRequest request);

        Task<CustomAPIResponse<OrderViewModel>> GetOrderById(int orderId);

        Task<CustomAPIResponse<OrderOverviewViewModel>> GetOverviewStatictis();
    }
}