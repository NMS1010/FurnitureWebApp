﻿using Domain.Entities;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.Utilities.Constants.Users;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _fileStorage;

        public UserService(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IFileStorageService fileStorage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _fileStorage = fileStorage;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return null;
            var res = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);
            if (!res.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(';', roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(bool, string)> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                DateOfBirth = request.Dob,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                Status = request.Status,
                DateCreated = DateTime.Now,
                Address = request.Address,
                DateUpdated = DateTime.Now,
                Avatar = await _fileStorage.SaveFile(request.Avatar),
            };
            var res = await _userManager.CreateAsync(user, request.Password);
            if (res.Succeeded)
                return (true, "successfull");
            string error = "";
            res.Errors.ToList().ForEach(x => error += (x.Description + "/n"));
            return (false, error);
        }

        public async Task<PagedResult<UserViewModel>> RetrieveAll(UserGetPagingRequest request)
        {
            var users = await _userManager.Users.Include(u => u.WishItems)
                .Include(u => u.CartItems)
                .Include(u => u.WishItems)
                .Include(u => u.Orders)
                .ThenInclude(u => u.OrderItems)
                .ToListAsync();
            string keyWord = request.Keyword;
            if (!string.IsNullOrEmpty(keyWord))
            {
                users = (List<AppUser>)users.Where(a =>
                    a.UserName.Contains(keyWord) ||
                    a.PhoneNumber.Contains(keyWord)
                );
            }

            int totalRow = users.Count;

            var dt = users
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserViewModel()
                {
                    UserId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    UserName = x.UserName,
                    Address = x.Address,
                    Dob = x.DateOfBirth,
                    Gender = x.Gender,
                    Avatar = x.Avatar,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Status = x.Status,
                    Password = x.PasswordHash,
                    TotalCartItem = x.CartItems.Count,
                    TotalWishItem = x.WishItems.Count,
                    TotalOrders = x.Orders.Count,
                    TotalBought = x.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity)),
                    TotalCost = x.Orders.Sum(o => o.TotalPrice),
                    StatusCode = USER_STATUS.UserStatus[x.Status]
                })
                .ToList();

            return new PagedResult<UserViewModel>()
            {
                TotalItem = totalRow,
                Items = dt
            };
        }

        public async Task<UserViewModel> RetrieveById(string userId)
        {
            var x = await _userManager.FindByIdAsync(userId);
            if (x == null)
                return null;
            return new UserViewModel()
            {
                UserId = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                UserName = x.UserName,
                Address = x.Address,
                Dob = x.DateOfBirth,
                Gender = x.Gender,
                Avatar = x.Avatar,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                Status = x.Status,
                Password = x.PasswordHash,
                TotalCartItem = x.CartItems.Count,
                TotalWishItem = x.WishItems.Count,
                TotalOrders = x.Orders.Count,
                TotalBought = x.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity)),
                TotalCost = x.Orders.Sum(o => o.TotalPrice),
                StatusCode = USER_STATUS.UserStatus[x.Status]
            };
        }
    }
}