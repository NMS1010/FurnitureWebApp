using FurnitureWeb.APICaller.User;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize]
    [Route("~/users")]
    public class UserCheckController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserCheckController(IUserAPIClient userAPIClient, RoleManager<IdentityRole> roleManager)
        {
            _userAPIClient = userAPIClient;
            _roleManager = roleManager;
        }

        [HttpPost("check-add")]
        public async Task<IActionResult> CheckNewUser(UserCheckNewRequest request)
        {
            var res = await _userAPIClient.CheckNewUser(request);
            return Ok(res);
        }

        [HttpPost("check-edit")]
        public async Task<IActionResult> CheckEditUser(UserCheckEditRequest request)
        {
            var res = await _userAPIClient.CheckEditUser(request);
            return Ok(res);
        }
    }
}