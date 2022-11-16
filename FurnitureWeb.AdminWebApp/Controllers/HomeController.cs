using FurnitureWeb.AdminWebApp.Models;
using FurnitureWeb.APICaller.User;
using FurnitureWeb.Utilities.Constants.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("~/admin/home")]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString(SystemConstants.AppSettings.BearerTokenSession);
            if (session == null)
                return Redirect("~/admin/login");
            return View();
        }
    }
}