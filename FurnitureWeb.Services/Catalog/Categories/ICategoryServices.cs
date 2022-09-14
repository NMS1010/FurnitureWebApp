using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Categories
{
    public interface ICategoryServices
    {
        Task<int> Create(CategoryCreateRequest request);

        Task<PagedResult<CategoryViewModel>> RetrieveAll(CategoryGetPagingRequest request);

        Task<int> Update(CategoryUpdateRequest request);

        Task<int> Delete(int categoryId);

        Task<CategoryViewModel> RetrieveById(int categoryId);
    }
}