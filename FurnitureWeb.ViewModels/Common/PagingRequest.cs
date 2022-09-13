using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Common
{
    public class PagingRequest : RequestBase
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public Func<int?, string> GenerateUrl { get; set; }
    }
}