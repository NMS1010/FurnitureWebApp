using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Category
{
    public interface ICategoryAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateCategory(CategoryCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateCategory(CategoryUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<CategoryViewModel>>> GetAllCategoryAsync(CategoryGetPagingRequest request);

        Task<CustomAPIResponse<CategoryViewModel>> GetCategoryById(int categoryId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteCategory(int categoryId);
    }
}