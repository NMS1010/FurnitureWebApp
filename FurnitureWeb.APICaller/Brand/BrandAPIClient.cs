using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Catalog.Products;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Brand
{
    public class BrandAPIClient : BaseAPIClient, IBrandAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public BrandAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateBrand(BrandCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.BrandName), "brandName");
            requestContent.Add(new StringContent(request.Origin), "origin");
            if (request.Image != null)
            {
                byte[] dataImage;
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    dataImage = stream.ReadBytes((int)request.Image.Length);
                }
                requestContent.Add(new ByteArrayContent(dataImage), "image", request.Image.FileName);
            }
            var response = await httpClient.PostAsync($"/api/brands/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body);
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> Delete(int brandId)
        {
            return await Delete($"/api/brands/delete/{brandId}");
        }

        public async Task<CustomAPIResponse<PagedResult<BrandViewModel>>> GetAllAsync(BrandGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<BrandViewModel>>>($"/api/brands/all");
        }

        public async Task<CustomAPIResponse<BrandViewModel>> GetById(int brandId)
        {
            return await GetAsync<CustomAPIResponse<BrandViewModel>>($"/api/brands/{brandId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateBrand(BrandUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.BrandId.ToString()), "brandId");
            requestContent.Add(new StringContent(request.BrandName), "brandName");
            requestContent.Add(new StringContent(request.Origin), "origin");

            byte[] imageBytes;
            if (request.Image != null)
            {
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    imageBytes = stream.ReadBytes((int)request.Image.Length);
                }
            }
            else
            {
                CustomAPIResponse<BrandViewModel> res = await GetById(request.BrandId);
                if (!res.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(res.StatusCode, res.Errors);
                string path = _configuration["BaseAddress"] + res.Data.Image;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", request.Image.FileName);

            var response = await httpClient.PutAsync($"/api/brands/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body);
        }
    }
}