using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Route("~/admin/")]
    public class LoginController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;

        public LoginController(IUserAPIClient userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }

        [Route("login")]
        public IActionResult Index(string error = null)
        {
            ViewData["error"] = error;
            return View("Index");
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> SignOut()
        {
            HttpContext.Session.Remove(SystemConstants.AppSettings.BearerTokenSession);
            await HttpContext.SignOutAsync("AdminAuth");
            HttpContext.Response.Cookies.Delete("X-Access-Token-Admin");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var res = await _userAPIClient.Login(request);
            if (!res.IsSuccesss)
            {
                return Index("Username/Password không chính xác");
            }

            string token = res.Data;
            if (token == "banned")
            {
                return Index("Tài khoản đang bị cấm hoạt động");
            }
            var userPrincipal = ValidateJWT(token);
            var roles = userPrincipal.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
            if (!roles.Any(x => x.ToLower() == "admin"))
            {
                return Index("Tài khoản không có quyền truy cập");
            }
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = request.RememberMe
            };
            await HttpContext.SignInAsync(
                         "AdminAuth",
                         userPrincipal,
                         authProperties
                         );
            Response.Cookies.Append("X-Access-Token-Admin", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Lax });
            return Redirect("/admin/home");
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