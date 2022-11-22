using FurnitureWeb.Services.Common.Interfaces;
using FurnitureWeb.ViewModels.System.Roles;

namespace FurnitureWeb.Services.System.Roles
{
    public interface IRoleServices : IModifyEntity<RoleCreateRequest, RoleUpdateRequest, string>,
        IRetrieveEntity<RoleViewModel, RoleGetPagingRequest, string>
    {
    }
}