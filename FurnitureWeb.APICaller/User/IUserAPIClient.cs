using FurnitureWeb.ViewModels.Catalog.Brands;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.User
{
    public interface IUserAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> Register(RegisterRequest request);

        Task<CustomAPIResponse<string>> Login(LoginRequest request);

        Task<CustomAPIResponse<PagedResult<UserViewModel>>> GetAllAsync(UserGetPagingRequest request);

        Task<CustomAPIResponse<UserViewModel>> GetById(string userId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteBrand(string userId);

        Task<CustomAPIResponse<NoContentAPIResponse>> Update(UserUpdateRequest request);

        Task<CustomAPIResponse<UserViewModel>> RetrieveByClaimsPrincipal(ClaimsPrincipal claims);
    }
}