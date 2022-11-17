using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FurnitureWeb.Utilities.Constants.Systems;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using FurnitureWeb.ViewModels.Common;

namespace FurnitureWeb.APICaller.Common
{
    public class BaseAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public BaseAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return (TResponse)JsonConvert.DeserializeObject(body,
                        typeof(TResponse));
            }
            return default;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> Delete(string url)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var response = await httpClient.DeleteAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body,
                    typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}