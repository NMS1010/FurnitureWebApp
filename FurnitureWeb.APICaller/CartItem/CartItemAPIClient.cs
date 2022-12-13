using FurnitureWeb.APICaller.Common;
using FurnitureWeb.ViewModels.Catalog.CartItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.CartItem
{
    public class CartItemAPIClient : BaseAPIClient, ICartItemAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CartItemAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<string>> AddProductToCart(CartItemCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.UserId), "userId" },
                { new StringContent(request.ProductId.ToString()), "productId" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.Quantity.ToString()), "quantity" }
            };

            var response = await httpClient.PostAsync($"/api/cartItems/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<string>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<string>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteCartItem(int cartItemId)
        {
            return await Delete($"/api/cartItems/delete/{cartItemId}");
        }

        public async Task<CustomAPIResponse<PagedResult<CartItemViewModel>>> GetAllCartItemByUser(string userId)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(userId), "userId" }
            };
            var response = await httpClient.PostAsync($"/api/cartItems/all", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<PagedResult<CartItemViewModel>>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<PagedResult<CartItemViewModel>>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateItemQuantity(CartItemUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.CartItemId.ToString()), "cartItemId" },
                { new StringContent(request.Quantity.ToString()), "quantity" },
                { new StringContent(request.Status.ToString()), "status" }
            };

            var response = await httpClient.PutAsync($"/api/cartItems/update", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}