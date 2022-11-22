using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Role
{
    public class RoleAPIClient : BaseAPIClient, IRoleAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public RoleAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateRole(RoleCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.RoleName), "roleName" }
            };

            var response = await httpClient.PostAsync($"/api/roles/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteRole(string roleId)
        {
            return await Delete($"/api/roles/delete/{roleId}");
        }

        public async Task<CustomAPIResponse<PagedResult<RoleViewModel>>> GetAllRoleAsync(RoleGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<RoleViewModel>>>($"/api/roles/all");
        }

        public async Task<CustomAPIResponse<RoleViewModel>> GetRoleById(string roleId)
        {
            return await GetAsync<CustomAPIResponse<RoleViewModel>>($"/api/roles/{roleId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateRole(RoleUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.RoleId), "roleId" },
                { new StringContent(request.RoleName), "roleName" }
            };

            var response = await httpClient.PutAsync($"/api/roles/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}