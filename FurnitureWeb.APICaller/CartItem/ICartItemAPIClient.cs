using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.CartItem
{
    public interface ICartItemAPIClient
    {
        Task<CustomAPIResponse<string>> AddProductToCart(CartItemCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateItemQuantity(CartItemUpdateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteCartItem(int cartItemId);

        Task<CustomAPIResponse<PagedResult<CartItemViewModel>>> GetAllCartItemByUser(string userId);
    }
}