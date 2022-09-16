using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.OrderItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Services.Catalog.OrderItems
{
    public interface IOrderItemServices : IModifyEntity<OrderItemCreateRequest, OrderItemUpdateRequest, int>,
        IRetrieveEntity<OrderItemViewModel, OrderItemGetPagingRequest, int>
    {
    }
}