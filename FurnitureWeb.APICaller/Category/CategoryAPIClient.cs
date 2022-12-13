using FurnitureWeb.APICaller.Common;
using FurnitureWeb.ViewModels.Catalog.Categories;
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

namespace FurnitureWeb.APICaller.Category
{
    public class CategoryAPIClient : BaseAPIClient, ICategoryAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public CategoryAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateCategory(CategoryCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.Name), "name");
            requestContent.Add(new StringContent(request.Content), "content");
            if (request.ParentCategoryId.HasValue)
                requestContent.Add(new StringContent(request.ParentCategoryId.Value.ToString()), "parentCategoryId");
            if (request.Image != null)
            {
                byte[] dataImage;
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    dataImage = stream.ReadBytes((int)request.Image.Length);
                }
                requestContent.Add(new ByteArrayContent(dataImage), "image", request.Image.FileName);
            }
            var response = await httpClient.PostAsync($"/api/categories/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteCategory(int categoryId)
        {
            return await Delete($"/api/categories/delete/{categoryId}");
        }

        public async Task<CustomAPIResponse<PagedResult<CategoryViewModel>>> GetAllCategoryAsync(CategoryGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<CategoryViewModel>>>($"/api/categories/all");
        }

        public async Task<CustomAPIResponse<CategoryViewModel>> GetCategoryById(int categoryId)
        {
            return await GetAsync<CustomAPIResponse<CategoryViewModel>>($"/api/categories/{categoryId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateCategory(CategoryUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent();

            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
            requestContent.Add(new StringContent(request.Name), "name");
            requestContent.Add(new StringContent(request.Content), "content");
            if (request.ParentCategoryId.HasValue)
                requestContent.Add(new StringContent(request.ParentCategoryId.Value.ToString()), "parentCategoryId");

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
                CustomAPIResponse<CategoryViewModel> res = await GetCategoryById(request.CategoryId);
                if (!res.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(res.StatusCode, res.Errors);
                string path = _configuration["BaseAddress"] + res.Data.Image;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
                fileName = Path.GetFileName(res.Data.Image);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", fileName);

            var response = await httpClient.PutAsync($"/api/categories/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}