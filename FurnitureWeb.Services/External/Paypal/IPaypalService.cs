using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;

namespace FurnitureWeb.Services.External.Paypal
{
    public interface IPaypalService
    {
        string PaypalCheckout(PagedResult<CartItemViewModel> products, OrderCreateRequest orderCreateRequest, string hostname);

        bool ExecutePayment(string payerID, string paymentID);
    }
}