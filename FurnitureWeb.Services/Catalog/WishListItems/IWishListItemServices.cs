using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.WishListItems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Services.Catalog.WishListItems
{
    public interface IWishListItemServices : IModifyEntity<WishListItemCreateRequest, WishListItemUpdateRequest, int>,
        IRetrieveEntity<WishListItemViewModel, WishListItemGetPagingRequest, int>
    {
    }
}