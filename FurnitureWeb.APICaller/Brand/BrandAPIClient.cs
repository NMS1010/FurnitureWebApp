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

        public async Task<bool> CreateBrand(BrandCreateRequest request)
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

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int brandId)
        {
            return await Delete($"/api/brands/delete/{brandId}");
        }

        public async Task<PagedResult<BrandViewModel>> GetAllAsync(BrandGetPagingRequest request)
        {
            return await GetAsync<PagedResult<BrandViewModel>>($"/api/brands/all");
        }

        public async Task<BrandViewModel> GetById(int brandId)
        {
            return await GetAsync<BrandViewModel>($"/api/brands/{brandId}");
        }

        public async Task<bool> UpdateBrand(BrandUpdateRequest request)
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
                BrandViewModel brand = await GetById(request.BrandId);
                string path = _configuration["BaseAddress"] + brand.Image;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", request.Image.FileName);

            var response = await httpClient.PutAsync($"/api/brands/update", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}