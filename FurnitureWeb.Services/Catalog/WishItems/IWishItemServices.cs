using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Services.Catalog.WishItems
{
    public interface IWishItemServices : IModifyEntity<WishItemCreateRequest, WishItemUpdateRequest, int>,
        IRetrieveEntity<WishItemViewModel, WishItemGetPagingRequest, int>
    {
    }
}