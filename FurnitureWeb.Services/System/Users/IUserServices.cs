using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.System.Users
{
    public interface IUserServices
    {
        Task<string> Authenticate(LoginRequest request);

        Task<string> AuthenticateWithGoogle(string email, string loginProvider, string providerKey);

        Task<(bool, string)> Register(RegisterRequest request);

        Task<bool> VerifyToken(string email, string token);

        Task<PagedResult<UserViewModel>> RetrieveAll(UserGetPagingRequest request);

        Task<UserViewModel> RetrieveById(string userId);

        Task<(bool, string)> Update(UserUpdateRequest request);

        Task<int> Delete(string userId);

        Task<List<string>> CheckNewUser(UserCheckNewRequest request);

        Task<List<string>> CheckEditUser(UserCheckEditRequest request);
    }
}