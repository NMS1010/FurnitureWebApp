using FurnitureWeb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.System.Users
{
    public class UserGetPagingRequest : PagingRequest
    {
        public string Keyword { get; set; }
    }
}