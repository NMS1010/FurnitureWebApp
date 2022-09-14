using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Common
{
    public class PagingRequest : RequestBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
}