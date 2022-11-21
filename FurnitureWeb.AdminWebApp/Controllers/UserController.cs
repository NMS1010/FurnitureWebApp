using FurnitureWeb.APICaller.User;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("~/admin/users")]
    public class UserController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserAPIClient userAPIClient, RoleManager<IdentityRole> roleManager)
        {
            _userAPIClient = userAPIClient;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(bool error = false)
        {
            var res = await _userAPIClient.GetAllUserAsync(new UserGetPagingRequest());
            if (error || !res.IsSuccesss)
                ViewData["Error"] = "error";
            ViewData["roles"] = await _roleManager.Roles.ToListAsync();
            return View(res.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            var res = await _userAPIClient.Register(request);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccesss });
        }

        [HttpGet("delete/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var res = await _userAPIClient.DeleteUser(userId);
            return RedirectToAction(nameof(Index), new { error = !res.IsSuccesss });
        }

        [Route("detail/{userId}")]
        public async Task<IActionResult> GetDetail(string userId)
        {
            var res = await _userAPIClient.GetUserById(userId);
            if (!res.IsSuccesss)
            {
                ViewData["error"] = res.Errors;
            }
            ViewData["roles"] = await _roleManager.Roles.ToListAsync();
            return View("UserDetails", res.Data);
        }

        [Route("get/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetById(string userId)
        {
            var res = await _userAPIClient.GetUserById(userId);
            if (!res.IsSuccesss)
                return NotFound(res.Errors);
            return Ok(res.Data);
        }

        [HttpPost("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            var res = await _userAPIClient.UpdateUser(request);
            return Redirect($"detail/{request.UserId}");
        }
    }
}