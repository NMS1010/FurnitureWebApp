using FurnitureWeb.ViewModels.Common;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Common.Interfaces
{
    public interface IRetrieveEntity<ReturnType, Entity, EntityId>
    {
        Task<PagedResult<ReturnType>> RetrieveAll(Entity entity);

        Task<ReturnType> RetrieveById(EntityId entity);
    }
}