﻿using Domain.Entities;
using FurnitureWeb.APICaller.Common;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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

        public async Task<PagedResult<UserViewModel>> GetAllAsync(UserGetPagingRequest request)
        {
            return await GetAsync<PagedResult<UserViewModel>>("/api/users/all");
        }

        public async Task<UserViewModel> GetById(string userId)
        {
            return await GetAsync<UserViewModel>($"/api/users/{userId}");
        }

        public async Task<string> Login(LoginRequest request)
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

            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(_configuration["BaseAddress"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            request.Roles.Clear();
            request.Roles.Add("Customer");
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
                { new StringContent(request.Roles.ToString()), "Roles" }
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

            return response.IsSuccessStatusCode;
        }

        public async Task<UserViewModel> RetrieveByClaimsPrincipal(ClaimsPrincipal claims)
        {
            var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await GetById(userId);
            if (user == null)
                return new UserViewModel();
            return user;
        }

        public async Task<bool> Update(UserUpdateRequest request)
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
                { new StringContent(request.Password), "Password" },
                { new StringContent(request.ConfirmPassword), "ConfirmPassword" },
                { new StringContent(request.PhoneNumber), "PhoneNumber" },
                { new StringContent(request.UserName), "UserName" },
                { new StringContent(request.Roles.ToString()), "Roles" }
            };
            byte[] userBytes;
            if (request.Avatar != null)
            {
                using (var stream = new BinaryReader(request.Avatar.OpenReadStream()))
                {
                    userBytes = stream.ReadBytes((int)request.Avatar.Length);
                }
            }
            else
            {
                UserViewModel user = await GetById(request.UserId);
                string path = _configuration["BaseAddress"] + user.Avatar;
                WebClient webClient = new WebClient();
                userBytes = webClient.DownloadData(path);
            }
            requestContent.Add(new ByteArrayContent(userBytes), "Avatar", request.Avatar.FileName);

            var response = await httpClient.PutAsync($"/api/users/update", requestContent);

            return response.IsSuccessStatusCode;
        }
    }
}