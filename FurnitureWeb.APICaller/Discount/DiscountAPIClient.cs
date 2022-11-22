using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Catalog.Discounts;
using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Discount
{
    public class DiscountAPIClient : BaseAPIClient, IDiscountAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DiscountAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateDiscount(DiscountCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.DiscountCode), "discountCode" },
                { new StringContent(request.DiscountValue.ToString()), "discountValue" },
                { new StringContent(request.Quantity.ToString()), "quantity" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.StartDate.ToString()), "startDate" },
                { new StringContent(request.EndDate.ToString()), "endDate" }
            };

            var response = await httpClient.PostAsync($"/api/discounts/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteDiscount(int discountId)
        {
            return await Delete($"/api/discounts/delete/{discountId}");
        }

        public async Task<CustomAPIResponse<PagedResult<DiscountViewModel>>> GetAllDiscountAsync(DiscountGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<DiscountViewModel>>>($"/api/discounts/all");
        }

        public async Task<CustomAPIResponse<DiscountViewModel>> GetDiscountById(int discountId)
        {
            return await GetAsync<CustomAPIResponse<DiscountViewModel>>($"/api/discounts/{discountId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateDiscount(DiscountUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.DiscountId.ToString()), "discountId" },
                { new StringContent(request.DiscountCode), "discountCode" },
                { new StringContent(request.DiscountValue.ToString()), "discountValue" },
                { new StringContent(request.Quantity.ToString()), "quantity" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.StartDate.ToString()), "startDate" },
                { new StringContent(request.EndDate.ToString()), "endDate" }
            };

            var response = await httpClient.PutAsync($"/api/discounts/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}