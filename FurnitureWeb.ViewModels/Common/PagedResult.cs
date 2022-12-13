using System.Collections.Generic;

namespace FurnitureWeb.ViewModels.Common
{
    public class PagedResult<T>
    {
        public int TotalItem { get; set; }
        public List<T> Items { get; set; }
    }
}