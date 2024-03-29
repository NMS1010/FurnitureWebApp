﻿using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.User
{
    public interface IUserAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> Register(RegisterRequest request);

        Task<CustomAPIResponse<string>> Login(LoginRequest request);

        Task<CustomAPIResponse<string>> VerifyToken(string email, string token);

        Task<CustomAPIResponse<string>> LoginWithGoogle(string email, string loginProvider, string providerKey);

        Task<CustomAPIResponse<PagedResult<UserViewModel>>> GetAllUserAsync(UserGetPagingRequest request);

        Task<CustomAPIResponse<UserViewModel>> GetUserById(string userId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteUser(string userId);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateUser(UserUpdateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> CheckNewUser(UserCheckNewRequest request);

        Task<CustomAPIResponse<string>> CheckEmail(string email);

        Task<CustomAPIResponse<string>> ForgotPassword(string email, string host);

        Task<CustomAPIResponse<string>> ResetPassword(string email, string token, string password);

        Task<CustomAPIResponse<NoContentAPIResponse>> CheckEditUser(UserCheckEditRequest request);

        Task<CustomAPIResponse<UserViewModel>> RetrieveByClaimsPrincipal(ClaimsPrincipal claims);
    }
}