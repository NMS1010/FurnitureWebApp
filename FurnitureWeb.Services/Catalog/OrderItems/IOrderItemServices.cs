using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.OrderItems
{
    public interface IOrderItemServices : IModifyEntity<OrderItemCreateRequest, OrderItemUpdateRequest, int>,
        IRetrieveEntity<OrderItemViewModel, OrderItemGetPagingRequest, int>
    {
        Task<PagedResult<OrderItemViewModel>> RetrieveByOrderId(int orderId);
    }
}