using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureWeb.AdminWebApp.Controllers
{
    [Authorize]
    [Route("~/admin/users")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}