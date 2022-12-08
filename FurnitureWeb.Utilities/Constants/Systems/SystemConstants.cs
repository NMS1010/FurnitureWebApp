using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Systems
{
    public class SystemConstants
    {
        public class AppSettings
        {
            public static string BearerTokenSession = "BearerToken";
        }

        public class UserRoles
        {
            public static Dictionary<int, string> Roles = new Dictionary<int, string>()
            {
                {1,"Admin" },
                {2,"Customer" }
            };
        }
    }
}