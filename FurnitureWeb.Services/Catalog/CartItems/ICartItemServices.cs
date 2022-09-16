using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Services.Catalog.CartItems
{
    public interface ICartItemServices : IModifyEntity<CartItemCreateRequest, CartItemUpdateRequest, int>,
        IRetrieveEntity<CartItemViewModel, CartItemGetPagingRequest, int>
    {
    }
}