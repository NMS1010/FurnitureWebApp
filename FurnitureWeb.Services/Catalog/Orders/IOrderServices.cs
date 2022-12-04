using Domain.Entities;
using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Orders
{
    public interface IOrderServices : IModifyEntity<OrderCreateRequest, OrderUpdateRequest, int>,
        IRetrieveEntity<OrderViewModel, OrderGetPagingRequest, int>
    {
        Task<OrderOverviewViewModel> GetOverviewStatictis();
    }
}