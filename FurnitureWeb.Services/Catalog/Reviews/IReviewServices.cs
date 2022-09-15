using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Catalog.Reviews;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Reviews
{
    public interface IReviewServices : IModifyEntity<ReviewCreateRequest, ReviewUpdateRequest, int>,
        IRetrieveEntity<ReviewViewModel, ReviewGetPagingRequest, int>
    {
    }
}