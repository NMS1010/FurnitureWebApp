using FurnitureWeb.Services.System.Users;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resToken = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(resToken))
            {
                return BadRequest("Username/password is incorrect");
            }
            return Ok(resToken);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            (var res, var status) = await _userService.Register(request);
            if (!res)
            {
                return BadRequest(status);
            }
            return Ok(status);
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] UserGetPagingRequest request)
        {
            return Ok(await _userService.RetrieveAll(request));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> RetrieveById(string userId)
        {
            return Ok(await _userService.RetrieveById(userId));
        }

        [HttpPut("update")]
        public async Task<IActionResult> RetrieveAll([FromForm] UserUpdateRequest request)
        {
            return Ok(await _userService.Update(request));
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> RetrieveAll(string userId)
        {
            return Ok(await _userService.Delete(userId));
        }
    }
}