using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using PayPal.Api.OpenIdConnect;
using PayPal.Api;
using PayPal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.External.Paypal
{
    public class PaypalService : IPaypalService
    {
        private readonly string CLIENT_ID = "AR52hDoJM7wVzALLe_nPlzKxMS8CTJfoUAeRt9IocXy4c4EDG0T2KPBwG4f38RtLYz9Pem_DDPkT0-ID";
        private readonly string CLIENT_SECRET = "EHv86VipUu7nzvBRTw1Sff3BeTjPkmIjekg_Uemr-T3im9MzUJQusec2B8boB2MxwklloYQNL0RfpQqD";
        private readonly decimal VND_TO_USD = 24455;

        public bool ExecutePayment(string payerID, string paymentID)
        {
            try
            {
                OAuthTokenCredential tokenCredential = new OAuthTokenCredential(CLIENT_ID, CLIENT_SECRET);
                string accessToken = tokenCredential.GetAccessToken();
                var apiContext = new APIContext(accessToken);

                var paymentExecution = new PaymentExecution() { payer_id = payerID };
                var payment = new Payment() { id = paymentID };
                var executedPayment = payment.Execute(apiContext, paymentExecution);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string PaypalCheckout(PagedResult<CartItemViewModel> products, OrderCreateRequest orderCreateRequest, string hostname)
        {
            OAuthTokenCredential tokenCredential = new OAuthTokenCredential(CLIENT_ID, CLIENT_SECRET);
            string accessToken = tokenCredential.GetAccessToken();
            var apiContext = new APIContext(accessToken);

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            decimal total = 0;
            foreach (var item in products.Items)
            {
                if (item.Quantity == 0)
                    continue;
                decimal price = Math.Round(item.UnitPrice / VND_TO_USD, 2);
                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = price.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = "sku",
                    tax = "0",
                });
                total += price;
            }
            var paypalOrderId = DateTime.Now.Ticks;
            var payment = new Payment()
            {
                intent = "authorize",
                transactions = new List<Transaction>()
                {
                    new Transaction()
                    {
                        amount = new Amount()
                        {
                            total = total.ToString(),
                            currency = "USD",
                            details = new Details()
                            {
                                tax = "0",
                                shipping = "0",
                                subtotal = total.ToString()
                            }
                        },
                        item_list = itemList,
                        description = $"Invoice #{paypalOrderId}",
                        invoice_number = paypalOrderId.ToString()
                    }
                },
                redirect_urls = new RedirectUrls()
                {
                    cancel_url = $"{hostname}/checkout",
                    return_url = $"{hostname}/checkout/paypal/success"
                },
                payer = new Payer()
                {
                    payment_method = "paypal"
                }
            };
            var createdPayment = payment.Create(apiContext);
            try
            {
                var links = createdPayment.links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    Links lnk = links.Current;
                    if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the paypalredirect URL to which user will be redirected for payment
                        paypalRedirectUrl = lnk.href;
                    }
                }
                return paypalRedirectUrl;
            }
            catch
            {
                return null;
            }
        }
    }
}