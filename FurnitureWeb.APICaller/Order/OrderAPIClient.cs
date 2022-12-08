using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Catalog.Orders;

using FurnitureWeb.ViewModels.Catalog.Orders;

using FurnitureWeb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Order
{
    public class OrderAPIClient : BaseAPIClient, IOrderAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public OrderAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<PagedResult<OrderViewModel>>> GetAllOrderAsync(OrderGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<OrderViewModel>>>($"/api/orders/all");
        }

        public async Task<CustomAPIResponse<OrderViewModel>> GetOrderById(int orderId)
        {
            return await GetAsync<CustomAPIResponse<OrderViewModel>>($"/api/orders/{orderId}");
        }

        public async Task<CustomAPIResponse<OrderOverviewViewModel>> GetOverviewStatictis()
        {
            return await GetAsync<CustomAPIResponse<OrderOverviewViewModel>>("/api/orders/statictis");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateOrder(OrderUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.OrderId.ToString()), "orderId" },
                { new StringContent(request.Status.ToString()), "status" }
            };

            var response = await httpClient.PutAsync($"/api/orders/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}