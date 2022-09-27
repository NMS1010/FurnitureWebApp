using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime DateCreated { get; set; }
        public int Status { get; set; }
        public HashSet<Review> Reviews { get; set; }
        public HashSet<Order> Orders { get; set; }
        public HashSet<CartItem> CartItems { get; set; }
        public HashSet<WishList> WishLists { get; set; }
    }
}