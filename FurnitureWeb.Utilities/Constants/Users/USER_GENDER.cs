using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.Utilities.Constants.Users
{
    public class USER_GENDER
    {
        public static string MALE = "Nam";
        public static string FEMALE = "Nữ";
        public static string OTHER = "Khác";

        public static List<string> Gender = new List<string>()
        {
            MALE, FEMALE, OTHER
        };
    }
}