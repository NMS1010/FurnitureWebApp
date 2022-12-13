using FurnitureWeb.APICaller.Common;
using FurnitureWeb.ViewModels.Catalog.Wishtems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.WishItem
{
    public class WishItemAPIClient : BaseAPIClient, IWishItemAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public WishItemAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<string>> AddProductToWish(WishItemCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.UserId), "userId" },
                { new StringContent(request.ProductId.ToString()), "productId" },
                { new StringContent(request.Status.ToString()), "status" }
            };

            var response = await httpClient.PostAsync($"/api/wishItems/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<string>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<string>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteWishItem(int wishItemId)
        {
            return await Delete($"/api/wishItems/delete/{wishItemId}");
        }

        public async Task<CustomAPIResponse<PagedResult<WishItemViewModel>>> GetAllWishItemByUser(string userId)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(userId), "userId" }
            };
            var response = await httpClient.PostAsync($"/api/wishItems/all", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<PagedResult<WishItemViewModel>>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<PagedResult<WishItemViewModel>>));
        }
    }
}