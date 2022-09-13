using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Common
{
    public class PagedResult<T>
    {
        public int TotalItem { get; set; }
        public List<T> Items { get; set; }
    }
}