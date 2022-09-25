using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.System.Users
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
    }
}