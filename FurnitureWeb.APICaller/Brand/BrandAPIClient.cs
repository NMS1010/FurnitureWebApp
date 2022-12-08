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
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
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
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteBrand(int brandId)
        {
            return await Delete($"/api/brands/delete/{brandId}");
        }

        public async Task<CustomAPIResponse<PagedResult<BrandViewModel>>> GetAllBrandAsync(BrandGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<BrandViewModel>>>($"/api/brands/all");
        }

        public async Task<CustomAPIResponse<BrandViewModel>> GetBrandById(int brandId)
        {
            return await GetAsync<CustomAPIResponse<BrandViewModel>>($"/api/brands/{brandId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateBrand(BrandUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.BrandId.ToString()), "brandId");
            requestContent.Add(new StringContent(request.BrandName), "brandName");
            requestContent.Add(new StringContent(request.Origin), "origin");

            byte[] imageBytes;
            string fileName = "";
            if (request.Image != null)
            {
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    imageBytes = stream.ReadBytes((int)request.Image.Length);
                }
                fileName = request.Image.FileName;
            }
            else
            {
                CustomAPIResponse<BrandViewModel> res = await GetBrandById(request.BrandId);
                if (!res.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(res.StatusCode, res.Errors);
                string path = _configuration["BaseAddress"] + res.Data.Image;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
                fileName = Path.GetFileName(res.Data.Image);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", fileName);

            var response = await httpClient.PutAsync($"/api/brands/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}