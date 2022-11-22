using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Roles;
using System.Threading.Tasks;

namespace FurnitureWeb.APICaller.Role
{
    public interface IRoleAPIClient
    {
        Task<CustomAPIResponse<NoContentAPIResponse>> CreateRole(RoleCreateRequest request);

        Task<CustomAPIResponse<NoContentAPIResponse>> UpdateRole(RoleUpdateRequest request);

        Task<CustomAPIResponse<PagedResult<RoleViewModel>>> GetAllRoleAsync(RoleGetPagingRequest request);

        Task<CustomAPIResponse<RoleViewModel>> GetRoleById(string roleId);

        Task<CustomAPIResponse<NoContentAPIResponse>> DeleteRole(string roleId);
    }
}