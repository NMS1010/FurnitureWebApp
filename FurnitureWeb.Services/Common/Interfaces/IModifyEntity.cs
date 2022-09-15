using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.Common.Interfaces
{
    public interface IModifyEntity<CreateRequest, UpdateRequest, EntityId>
    {
        Task<int> Create(CreateRequest request);

        Task<int> Update(UpdateRequest request);

        Task<int> Delete(EntityId id);
    }
}