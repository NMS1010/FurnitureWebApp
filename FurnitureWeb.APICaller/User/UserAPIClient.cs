using Domain.Entities;
using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.User
{
    public class UserAPIClient : BaseAPIClient, IUserAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserAPIClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<CustomAPIResponse<PagedResult<UserViewModel>>> GetAllUserAsync(UserGetPagingRequest request)
        {
            return await GetAsync<CustomAPIResponse<PagedResult<UserViewModel>>>("/api/users/all");
        }

        public async Task<CustomAPIResponse<UserViewModel>> GetUserById(string userId)
        {
            return await GetAsync<CustomAPIResponse<UserViewModel>>($"/api/users/{userId}");
        }

        public async Task<CustomAPIResponse<string>> Login(LoginRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.UserName), "UserName" },
                { new StringContent(request.Password), "Password" },
                { new StringContent(request.RememberMe.ToString()), "RememberMe" }
            };

            var response = await httpClient.PostAsync($"/api/users/login", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<string>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<string>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> Register(RegisterRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.Email), "Email" },
                { new StringContent(request.Status.ToString()), "Status" },
                { new StringContent(request.Address), "Address" },
                { new StringContent(request.Dob.ToString("yyyy-MM-dd")), "Dob" },
                { new StringContent(request.LastName), "LastName" },
                { new StringContent(request.FirstName), "FirstName" },
                { new StringContent(request.Gender), "Gender" },
                { new StringContent(request.Password), "Password" },
                { new StringContent(request.ConfirmPassword), "ConfirmPassword" },
                { new StringContent(request.PhoneNumber), "PhoneNumber" },
                { new StringContent(request.UserName), "UserName" },
                { new StringContent(JsonConvert.SerializeObject(request.Roles)), "Roles" }
            };
            if (request.Avatar != null)
            {
                byte[] dataImage;
                using (var stream = new BinaryReader(request.Avatar.OpenReadStream()))
                {
                    dataImage = stream.ReadBytes((int)request.Avatar.Length);
                }
                requestContent.Add(new ByteArrayContent(dataImage), "Avatar", request.Avatar.FileName);
            }
            var response = await httpClient.PostAsync($"/api/users/register", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<UserViewModel>> RetrieveByClaimsPrincipal(ClaimsPrincipal claims)
        {
            var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await GetUserById(userId);
            return user;
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> UpdateUser(UserUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var requestContent = new MultipartFormDataContent
            {
                { new StringContent(request.UserId), "UserId" },
                { new StringContent(request.Email), "Email" },
                { new StringContent(request.Status.ToString()), "Status" },
                { new StringContent(request.Address), "Address" },
                { new StringContent(request.Dob.ToString("yyyy-MM-dd")), "Dob" },
                { new StringContent(request.LastName), "LastName" },
                { new StringContent(request.FirstName), "FirstName" },
                { new StringContent(request.Gender), "Gender" },
                { new StringContent(request.PhoneNumber), "PhoneNumber" },
                { new StringContent(request.UserName), "UserName" },
                { new StringContent(JsonConvert.SerializeObject(request.Roles)), "Roles" }
            };
            if (request.ConfirmPassword != null)
            {
                requestContent.Add(new StringContent(request.ConfirmPassword), "ConfirmPassword");
            }
            if (request.Password != null)
            {
                requestContent.Add(new StringContent(request.Password), "Password");
            }
            byte[] userBytes;
            string fileName = "";
            if (request.Avatar != null)
            {
                using (var stream = new BinaryReader(request.Avatar.OpenReadStream()))
                {
                    userBytes = stream.ReadBytes((int)request.Avatar.Length);
                }
                fileName = request.Avatar.FileName;
            }
            else
            {
                CustomAPIResponse<UserViewModel> user = await GetUserById(request.UserId);
                if (!user.IsSuccesss)
                    return CustomAPIResponse<NoContentAPIResponse>.Fail(user.StatusCode, user.Errors);
                string path = _configuration["BaseAddress"] + user.Data.Avatar;
                WebClient webClient = new WebClient();
                userBytes = webClient.DownloadData(path);
                fileName = Path.GetFileName(user.Data.Avatar);
            }
            requestContent.Add(new ByteArrayContent(userBytes), "Avatar", fileName);

            var response = await httpClient.PutAsync($"/api/users/update", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> DeleteUser(string userId)
        {
            return await Delete($"/api/users/delete/{userId}");
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CheckNewUser(UserCheckNewRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"/api/users/check-add", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }

        public async Task<CustomAPIResponse<NoContentAPIResponse>> CheckEditUser(UserCheckEditRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var requestContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"/api/users/check-edit", requestContent);
            var body = await response.Content.ReadAsStringAsync();
            return (CustomAPIResponse<NoContentAPIResponse>)JsonConvert.DeserializeObject(body, typeof(CustomAPIResponse<NoContentAPIResponse>));
        }
    }
}