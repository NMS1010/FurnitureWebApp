using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Categories
{
    public interface ICategoryServices : IModifyEntity<CategoryCreateRequest, CategoryUpdateRequest, int>,
        IRetrieveEntity<CategoryViewModel, CategoryGetPagingRequest, int>
    {
        List<CategoryViewModel> GetSubCategory(int categoryId);

        Task<PagedResult<CategoryViewModel>> GetParentCategory();
    }
}