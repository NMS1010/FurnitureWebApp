using Domain.Entities;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.Utilities.Constants.Users;
using FurnitureWeb.ViewModels.Common;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.WebApp.Controllers
{
    [Route("~/")]
    public class LoginController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(IUserAPIClient userAPIClient, IConfiguration configuration,
            SignInManager<AppUser> signInManager)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpGet("signin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login-google")]
        public async Task<IActionResult> GoogleLoginResponse()
        {
            HttpContext.Response.Cookies.Delete("X-Access-Token-User");
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var result = await _userAPIClient.LoginWithGoogle(info.Principal.FindFirst(ClaimTypes.Email).Value, info.LoginProvider, info.ProviderKey);
            if (result.IsSuccesss)
            {
                if (result.Data == "banned")
                    return Redirect("~/signin?banned");
                if (result.Data == "unconfirm")
                    return Redirect("~/signin?unconfirm");
                await AssignCookies(result.Data);
                return Redirect("/home");
            }
            GoogleUserViewModel g = new GoogleUserViewModel()
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                Family_name = info.Principal.FindFirst(ClaimTypes.Surname).Value,
                Given_name = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                LoginProvider = info.LoginProvider,
                ProviderKey = info.ProviderKey,
                Name = info.Principal.FindFirst(ClaimTypes.Name).Value,
            };
            ViewData["googleUser"] = g;
            return View("Register");
        }

        [HttpGet("login-with-google")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleLoginResponse", "Login");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("signout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SystemConstants.AppSettings.BearerTokenSession);
            HttpContext.SignOutAsync("UserAuth");
            HttpContext.Response.Cookies.Delete("X-Access-Token-User");
            return Redirect("/home");
        }

        [HttpPost("users/check-add")]
        public async Task<IActionResult> UserCheckAdd(UserCheckNewRequest request)
        {
            var res = await _userAPIClient.CheckNewUser(request);
            return Ok(res);
        }

        [HttpPost("users/check-edit")]
        public async Task<IActionResult> UserCheckEdit(UserCheckEditRequest request)
        {
            var res = await _userAPIClient.CheckEditUser(request);
            return Ok(res);
        }

        private async Task AssignCookies(string jwt)
        {
            var userPrincipal = ValidateJWT(jwt);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };
            await HttpContext.SignInAsync(
                         "UserAuth",
                         userPrincipal,
                         authProperties
                         );
            HttpContext.Response.Cookies.Append("X-Access-Token-User", jwt, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Lax });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var res = await _userAPIClient.Login(request);
            if (!res.IsSuccesss)
            {
                return Ok("error");
            }
            var token = res.Data;
            if (token == "banned")
                return Ok("banned");
            if (token == "unconfirm")
                return Ok("unconfirm");
            HttpContext.Response.Cookies.Delete("X-Access-Token-User");
            await AssignCookies(token);
            return Redirect("/home");
        }

        [HttpGet("register")]
        public IActionResult RegisterForm()
        {
            return View("Register");
        }

        [HttpGet("register-confirm")]
        public async Task<IActionResult> RegisterConfirm([FromQuery] string token, [FromQuery] string email)
        {
            var res = await _userAPIClient.VerifyToken(email, token);
            if (res.Data == null)
            {
                return Redirect($"signin?error");
            }
            return Redirect($"signin?{res.Data}");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            request.Status = USER_STATUS.ACTIVE;
            request.Roles = new string[] { "Customer" };
            request.Host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var res = await _userAPIClient.Register(request);

            if (!res.IsSuccesss)
            {
                return Redirect("signin?error");
            }
            return Redirect("signin?register");
        }

        private ClaimsPrincipal ValidateJWT(string jwt)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwt, validationParameters, out validatedToken);

            return principal;
        }
    }
}