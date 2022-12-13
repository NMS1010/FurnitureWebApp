using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.Discounts
{
    public interface IDiscountServices : IModifyEntity<DiscountCreateRequest, DiscountUpdateRequest, int>,
        IRetrieveEntity<DiscountViewModel, DiscountGetPagingRequest, int>
    {
        Task<string> ApllyDiscount(string discountCode);
    }
}