﻿using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Systems;
using FurnitureWeb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
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

        public LoginController(IUserAPIClient userAPIClient, IConfiguration configuration)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
        }

        [HttpGet("signin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SystemConstants.AppSettings.BearerTokenSession);
            HttpContext.SignOutAsync("UserAuth");
            HttpContext.Response.Cookies.Delete("X-Access-Token-User");
            return Redirect("/home");
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
            var userPrincipal = ValidateJWT(token);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = request.RememberMe,
            };
            await HttpContext.SignInAsync(
                         "UserAuth",
                         userPrincipal,
                         authProperties
                         );
            HttpContext.Response.Cookies.Append("X-Access-Token-User", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return Redirect("/home");
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