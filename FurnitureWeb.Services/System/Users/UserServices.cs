using Domain.EF;
using Domain.Entities;
using FurnitureWeb.Services.Catalog.Orders;
using FurnitureWeb.Services.Common.FileStorage;
using FurnitureWeb.Utilities.Constants.Users;
using FurnitureWeb.ViewModels.Catalog.Orders;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.System.Users
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _fileStorage;
        private readonly AppDbContext _context;
        private readonly IOrderServices _orderServices;

        public UserServices(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            IFileStorageService fileStorage,
            RoleManager<IdentityRole> roleManager, AppDbContext context, IOrderServices orderServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _fileStorage = fileStorage;
            _roleManager = roleManager;
            _context = context;
            _orderServices = orderServices;
        }

        private async Task<string> WriteJWT(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName + user.LastName),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return null;
            var res = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: true);
            if (!res.Succeeded)
                return null;
            if (user.Status == USER_STATUS.IN_ACTIVE)
                return "banned";

            return await WriteJWT(user);
        }

        public async Task<(bool, string)> Register(RegisterRequest request)
        {
            try
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
                    Avatar = await _fileStorage.SaveFile(request.Avatar)
                };
                var res = await _userManager.CreateAsync(user, request.Password);

                if (res.Succeeded)
                {
                    List<string> roles = new List<string>();
                    foreach (var roleId in JsonConvert.DeserializeObject<string[]>(request.Roles[0]))
                    {
                        var role = await _context.Roles.FindAsync(roleId);
                        if (role == null)
                        {
                            role = await _context.Roles.Where(x => x.Name.ToLower() == roleId.ToLower()).FirstOrDefaultAsync();
                        }
                        roles.Add(role.Name);
                    }
                    await _userManager.AddToRolesAsync(user, roles);
                    if (!string.IsNullOrEmpty(request.LoginProvider))
                    {
                        await _userManager.AddLoginAsync(user, new UserLoginInfo(request.LoginProvider, request.ProviderKey, request.LoginProvider));
                    }
                    return (true, "successfull");
                }

                string error = "";
                res.Errors.ToList().ForEach(x => error += (x.Description + "/n"));
                return (false, error);
            }
            catch
            {
                return (false, "Has error");
            }
        }

        public async Task<(bool, string)> Update(UserUpdateRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    return (false, "not found");
                string avatar = user.Avatar;

                user.FirstName = request.FirstName;
                user.DateOfBirth = request.Dob;
                user.Email = request.Email;
                user.LastName = request.LastName;
                user.UserName = request.UserName;
                user.PhoneNumber = request.PhoneNumber;
                user.Gender = request.Gender;
                user.Status = request.Status;
                user.DateUpdated = DateTime.Now;
                user.Address = request.Address;
                if (request.ConfirmPassword != null)
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.ConfirmPassword);
                user.Avatar = await _fileStorage.SaveFile(request.Avatar);
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded)
                {
                    await _fileStorage.DeleteFile(avatar);

                    await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    List<string> roles = new List<string>();
                    foreach (var roleId in JsonConvert.DeserializeObject<string[]>(request.Roles[0]))
                    {
                        var role = await _context.Roles.FindAsync(roleId);
                        if (role == null)
                        {
                            role = await _context.Roles.Where(x => x.Name.ToLower() == roleId.ToLower()).FirstOrDefaultAsync();
                        }
                        roles.Add(role.Name);
                    }
                    await _userManager.AddToRolesAsync(user, roles);

                    return (true, "successfull");
                }
                string error = "";
                res.Errors.ToList().ForEach(x => error += (x.Description + "/n"));
                return (false, error);
            }
            catch
            {
                return (false, "Has error");
            }
        }

        public async Task<PagedResult<UserViewModel>> RetrieveAll(UserGetPagingRequest request)
        {
            try
            {
                var users = await _userManager.Users
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
                        Dob = x.DateOfBirth.ToString("yyyy-MM-dd"),
                        Gender = x.Gender,
                        Avatar = x.Avatar,
                        DateCreated = x.DateCreated.ToString(),
                        DateUpdated = x.DateUpdated.ToString(),
                        Status = x.Status,
                        Password = x.PasswordHash,
                        TotalCartItem = x.CartItems.Count,
                        TotalWishItem = x.WishItems.Count,
                        TotalOrders = x.Orders.Count,
                        TotalBought = x.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity)),
                        TotalCost = x.Orders.Sum(o => o.TotalPrice),
                        StatusCode = USER_STATUS.UserStatus[x.Status],
                    }).ToList();

                foreach (var x in dt)
                {
                    x.RoleIds = (await _context.UserRoles.Where(u => u.UserId == x.UserId).Select(k => k.RoleId).ToListAsync());
                }

                return new PagedResult<UserViewModel>()
                {
                    TotalItem = totalRow,
                    Items = dt
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserViewModel> RetrieveById(string userId)
        {
            try
            {
                var x = await _userManager.Users
                    .Where(x => x.Id == userId)
                    .Include(u => u.WishItems)
                    .Include(u => u.CartItems)
                    .Include(u => u.WishItems)
                    .Include(u => u.Orders)
                    .ThenInclude(u => u.OrderItems)
                    .FirstOrDefaultAsync();
                if (x == null)
                    return null;
                var user = new UserViewModel()
                {
                    UserId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    UserName = x.UserName,
                    Address = x.Address,
                    Dob = x.DateOfBirth.ToString("yyyy-MM-dd"),
                    Gender = x.Gender,
                    Avatar = x.Avatar,
                    DateCreated = x.DateCreated.ToString(),
                    DateUpdated = x.DateUpdated.ToString(),
                    Status = x.Status,
                    Password = x.PasswordHash,
                    TotalCartItem = x.CartItems.Count,
                    TotalWishItem = x.WishItems.Count,
                    TotalOrders = x.Orders.Count,
                    TotalBought = x.Orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity)),
                    TotalCost = x.Orders.Sum(o => o.TotalPrice),
                    StatusCode = USER_STATUS.UserStatus[x.Status],
                };
                var roles = await _userManager.GetRolesAsync(x);
                user.RoleIds = (await _context.UserRoles.Where(u => u.UserId == x.Id).Select(k => k.RoleId).ToListAsync());
                user.Orders = new PagedResult<OrderViewModel>();
                List<OrderViewModel> orders = new List<OrderViewModel>();
                foreach (var order in x.Orders)
                {
                    orders.Add(await _orderServices.RetrieveById(order.OrderId));
                }
                user.Orders.Items = orders;
                user.Orders.TotalItem = orders.Count;
                return user;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> Delete(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return -1;
                user.Status = USER_STATUS.IN_ACTIVE;
                await _userManager.UpdateAsync(user);

                return 1;
            }
            catch { return -1; }
        }

        public async Task<List<string>> CheckNewUser(UserCheckNewRequest request)
        {
            List<string> exist = new List<string>();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                exist.Add("username");
            }
            user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                exist.Add("email");
            }

            user = await _context.Users.Where(x => x.PhoneNumber == request.Phone).FirstOrDefaultAsync();
            if (user != null)
            {
                exist.Add("phone");
            }

            return exist;
        }

        public async Task<List<string>> CheckEditUser(UserCheckEditRequest request)
        {
            List<string> exist = new List<string>();
            var user = await _context.Users.Where(x => x.UserName == request.UserName && x.Id != request.UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                exist.Add("username");
            }
            user = await _context.Users.Where(x => x.Email == request.Email && x.Id != request.UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                exist.Add("email");
            }

            user = await _context.Users.Where(x => x.PhoneNumber == request.Phone && x.Id != request.UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                exist.Add("phone");
            }
            user = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync();
            if (user != null && request.Password != null)
            {
                var res = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (res == PasswordVerificationResult.Failed && request.IsChangePassword)
                    exist.Add("password");
            }
            return exist;
        }

        public async Task<string> AuthenticateWithGoogle(string email, string loginProvider, string providerKey)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;
            var res = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, false);
            if (!res.Succeeded)
                return null;
            if (user.Status == USER_STATUS.IN_ACTIVE)
                return "banned";

            return await WriteJWT(user);
        }
    }
}