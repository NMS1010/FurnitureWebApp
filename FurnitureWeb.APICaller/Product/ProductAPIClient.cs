using FurnitureWeb.APICaller.Common;
using FurnitureWeb.ViewModels.Catalog.ProductImages;
using FurnitureWeb.ViewModels.Catalog.Products;
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

namespace FurnitureWeb.APICaller.Product
{
    public class ProductAPIClient : BaseAPIClient, IProductAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ProductAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateProduct(ProductCreateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.Name), "name" },
                { new StringContent(request.Description), "description" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.BrandId.ToString()), "brandId" },
                { new StringContent(request.CategoryId.ToString()), "categoryId" },
                { new StringContent(request.Price.ToString()), "price" },
                { new StringContent(request.Quantity.ToString()), "quantity" },
                { new StringContent(request.Origin), "origin" }
            };
            if (request.Image != null)
            {
                byte[] dataImage;
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    dataImage = stream.ReadBytes((int)request.Image.Length);
                }
                requestContent.Add(new ByteArrayContent(dataImage), "image", request.Image.FileName);
            }
            if (request.SubImages != null)
            {
                byte[] dataImage;
                foreach (var img in request.SubImages)
                {
                    using (var stream = new BinaryReader(img.OpenReadStream()))
                    {
                        dataImage = stream.ReadBytes((int)img.Length);
                    }
                    requestContent.Add(new ByteArrayContent(dataImage), "subImages", img.FileName);
                }
            }
            var response = await httpClient.PostAsync($"/api/products/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CreateProductImage(ProductImageCreateSingleRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.ProductId.ToString()), "productId" }
            };

            if (request.Image != null)
            {
                byte[] imageBytes;
                using (var stream = new BinaryReader(request.Image.OpenReadStream()))
                {
                    imageBytes = stream.ReadBytes((int)request.Image.Length);
                }
                requestContent.Add(new ByteArrayContent(imageBytes), "image", request.Image.FileName);
            }
            var response = await httpClient.PostAsync($"/api/products/image/add", requestContent);

            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteProduct(int productId)
        {
            return await Delete($"/api/products/delete/{productId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteProductImage(int productImageId)
        {
            return await Delete($"/api/products/images/delete/{productImageId}");
        }

        public async Task<CustomAPIResponse<PagedResult<ProductViewModel>>> GetAllProductAsync(ProductGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<ProductViewModel>>>($"/api/products/all?pageIndex={request.PageIndex}&pageSize={request.PageSize}&sortBy={request.SortBy}" +
                $"&keyword={request.Keyword}&categoryId={request.CategoryId}&brandId={request.BrandId}&minPrice={request.MinPrice}&maxPrice={request.MaxPrice}");
        }

        public async Task<CustomAPIResponse<PagedResult<ProductImageViewModel>>> GetAllProductImageByProductIdAsync(ProductImageGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<ProductImageViewModel>>>($"/api/products/{request.ProductId}/images/all");
        }

        public async Task<CustomAPIResponse<ProductViewModel>> GetProductById(int productId)
        {
            return await GetAsync<CustomAPIResponse<ProductViewModel>>($"/api/products/{productId}");
        }

        public async Task<CustomAPIResponse<ProductImageViewModel>> GetProductImageById(int productImageId)
        {
            return await GetAsync<CustomAPIResponse<ProductImageViewModel>>($"/api/products/images/{productImageId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateProduct(ProductUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.ProductId.ToString()), "productId" },
                { new StringContent(request.Name), "name" },
                { new StringContent(request.Description), "description" },
                { new StringContent(request.Status.ToString()), "status" },
                { new StringContent(request.BrandId.ToString()), "brandId" },
                { new StringContent(request.CategoryId.ToString()), "categoryId" },
                { new StringContent(request.Price.ToString()), "price" },
                { new StringContent(request.Quantity.ToString()), "quantity" },
                { new StringContent(request.Origin), "origin" }
            };

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
                CustomAPIResponse<ProductViewModel> res = await GetProductById(request.ProductId);
                if (!res.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(res.StatusCode, res.Errors);
                string path = _configuration["BaseAddress"] + res.Data.ImagePath;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
                fileName = Path.GetFileName(res.Data.ImagePath);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", fileName);

            var response = await httpClient.PutAsync($"/api/products/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateProductImage(ProductImageUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Request.Cookies["X-Access-Token-Admin"];
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.ProductImageId.ToString()), "productImageId" }
            };

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
                CustomAPIResponse<ProductImageViewModel> res = await GetProductImageById(request.ProductImageId);
                if (!res.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(res.StatusCode, res.Errors);
                string path = _configuration["BaseAddress"] + res.Data.Image;
                WebClient webClient = new WebClient();
                imageBytes = webClient.DownloadData(path);
                fileName = Path.GetFileName(res.Data.Image);
            }
            requestContent.Add(new ByteArrayContent(imageBytes), "image", fileName);

            var response = await httpClient.PutAsync($"/api/products/images/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}