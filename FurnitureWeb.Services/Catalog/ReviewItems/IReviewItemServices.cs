using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.ReviewItems
{
    public interface IReviewItemServices : IModifyEntity<ReviewItemCreateRequest, ReviewItemUpdateRequest, int>,
        IRetrieveEntity<ReviewItemViewModel, ReviewItemGetPagingRequest, int>
    {
        Task<int> ChangeReviewStatus(int reviewItemId);
    }
}