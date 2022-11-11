using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.System.Users
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);

        Task<(bool, string)> Register(RegisterRequest request);

        Task<PagedResult<UserViewModel>> RetrieveAll(UserGetPagingRequest request);

        Task<UserViewModel> RetrieveById(string userId);

        Task<(bool, string)> Update(UserUpdateRequest request);

        Task<int> Delete(string userId);
    }
}