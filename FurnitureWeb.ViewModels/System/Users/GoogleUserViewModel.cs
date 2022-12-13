﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.System.Users
{
    public class GoogleUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Verified_email { get; set; }
        public string Name { get; set; }
        public string Given_name { get; set; }
        public string Family_name { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
    }
}