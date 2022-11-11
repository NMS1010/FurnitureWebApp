using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Catalog.Categories;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Catalog.CartItems
{
    public interface ICartItemServices : IModifyEntity<CartItemCreateRequest, CartItemUpdateRequest, int>,
        IRetrieveEntity<CartItemViewModel, CartItemGetPagingRequest, int>
    {
        Task<PagedResult<CartItemViewModel>> RetrieveCartByUserId(string userId);

        Task UpdateQuantityByProductId(int productId, int quantity);

        Task<string> AddProductToCart(CartItemCreateRequest request);

        Task<bool> DeleteCartByUserId(string userId);

        Task<int> CanUpdateCartItemQuantity(int cartItemId, int quantity);
    }
}