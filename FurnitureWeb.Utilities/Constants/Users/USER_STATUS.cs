using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Users
{
    public class USER_STATUS
    {
        public static int IN_ACTIVE = 0;
        public static int ACTIVE = 1;

        public static Dictionary<int, string> UserStatus = new Dictionary<int, string>()
        {
            {IN_ACTIVE, "Đang hoạt động" },
            {ACTIVE, "Ngưng hoạt động" }
        };
    }
}