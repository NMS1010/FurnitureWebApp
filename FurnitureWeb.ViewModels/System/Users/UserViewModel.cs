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
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int Status { get; set; }
        public string StatusCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TotalBought { get; set; }
        public int TotalWishItem { get; set; }
        public int TotalCartItem { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalCost { get; set; }
        public List<int> RoleIds { get; set; }
    }
}