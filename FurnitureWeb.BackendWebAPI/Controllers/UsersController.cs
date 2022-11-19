using FurnitureWeb.Services.System.Users;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FurnitureWeb.BackendWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Username/password is incorrect"));
            }
            return Ok(CustomAPIResponse<string>.Success(resToken, StatusCodes.Status200OK));
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
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, status));
            }
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status201Created));
        }

        [HttpGet("all")]
        public async Task<IActionResult> RetrieveAll([FromQuery] UserGetPagingRequest request)
        {
            var res = await _userService.RetrieveAll(request);
            if (res.Items?.Count == 0)
                return NotFound(CustomAPIResponse<PagedResult<NoContentAPIResponse>>.Fail(StatusCodes.Status404NotFound, "Cannot get user list"));
            return Ok(CustomAPIResponse<PagedResult<UserViewModel>>.Success(res, StatusCodes.Status200OK));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> RetrieveById(string userId)
        {
            var res = await _userService.RetrieveById(userId);
            if (res == null)
                return NotFound(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status404NotFound, "Cannot find this user"));
            return Ok(CustomAPIResponse<UserViewModel>.Success(res, StatusCodes.Status200OK));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UserUpdateRequest request)
        {
            (var res, var status) = await _userService.Update(request);
            if (!res)
            {
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, status));
            }
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var count = await _userService.Delete(userId);
            if (count <= 0)
            {
                return BadRequest(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status400BadRequest, "Cannot delete this user"));
            }
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpPost("check-add")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckNewUser(UserCheckNewRequest request)
        {
            var res = await _userService.CheckNewUser(request);
            if (res.Count >= 0)
                return Ok(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status200OK, res));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }

        [HttpPost("check-edit")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEditUser(UserCheckEditRequest request)
        {
            var res = await _userService.CheckEditUser(request);
            if (res.Count >= 0)
                return Ok(CustomAPIResponse<NoContentAPIResponse>.Fail(StatusCodes.Status200OK, res));
            return Ok(CustomAPIResponse<NoContentAPIResponse>.Success(StatusCodes.Status200OK));
        }
    }
}