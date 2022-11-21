using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.Common
{
    public class PagingRequest : RequestBase
    {
        public string Keyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 1000;

        public string ColumnName { get; set; }
        public string TypeSort { get; set; }
        public string SortBy { get; set; }
    }
}