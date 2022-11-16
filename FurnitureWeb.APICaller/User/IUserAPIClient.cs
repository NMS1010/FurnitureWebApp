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
        Task<bool> Register(RegisterRequest request);

        Task<string> Login(LoginRequest request);

        Task<PagedResult<UserViewModel>> GetAllAsync(UserGetPagingRequest request);

        Task<UserViewModel> GetById(string userId);

        Task<bool> Delete(string userId);

        Task<bool> Update(UserUpdateRequest request);

        Task<UserViewModel> RetrieveByClaimsPrincipal(ClaimsPrincipal claims);
    }
}