using FurnitureWeb.APICaller.Common;
using FurnitureWeb.ViewModels.Catalog.ReviewItems;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.ReviewItem
{
    public class ReviewItemAPIClient : BaseAPIClient, IReviewItemAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ReviewItemAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> ChangeStatusReviewItem(int reviewItemId)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(reviewItemId.ToString()), "reviewItemId" }
            };

            var response = await httpClient.PutAsync($"/api/reviewItems/status/change", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateReviewItem(ReviewItemCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            if (session == null)
            {
                session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            }
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.Content), "content" },
                { new StringContent(request.ProductId.ToString()), "productId" },
                { new StringContent(request.Rating.ToString()), "rating" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.UserId.ToString()), "userId" },
            };

            var response = await httpClient.PostAsync($"/api/reviewItems/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteReviewItem(int reviewItemId)
        {
            return await Delete($"/api/reviewItems/delete/{reviewItemId}");
        }

        public async Task<CustomAPIResponse<PagedResult<ReviewItemViewModel>>> GetAllReviewItemAsync(ReviewItemGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<ReviewItemViewModel>>>($"/api/reviewItems/all");
        }

        public async Task<CustomAPIResponse<ReviewItemViewModel>> GetReviewItemById(int reviewItemId)
        {
            return await GetAsync<CustomAPIResponse<ReviewItemViewModel>>($"/api/reviewItems/{reviewItemId}");
        }

        public async Task<CustomAPIResponse<PagedResult<ReviewItemViewModel>>> GetReviewItemByUser(string userId, int productId)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(userId), "userId" },
                { new StringContent(productId.ToString()), "productId" },
            };

            var response = await httpClient.PostAsync($"/api/reviewItems/get-by-user", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<PagedResult<ReviewItemViewModel>>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<PagedResult<ReviewItemViewModel>>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateReviewItem(ReviewItemUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-User"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.ReviewItemId.ToString()), "reviewItemId" },
                { new StringContent(request.Content), "content" },
                { new StringContent(request.Rating.ToString()), "rating" },
                { new StringContent(request.Status.ToString()), "status" },
            };

            var response = await httpClient.PutAsync($"/api/reviewItems/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}