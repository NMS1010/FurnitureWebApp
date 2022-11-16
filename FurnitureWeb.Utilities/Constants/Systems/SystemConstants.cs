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

        public class SignedUser
        {
            public static string AdminUserSession = "Admin";
            public static string CustomerUserSession = "Customer";
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