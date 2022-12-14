using Microsoft.AspNetCore.Mvc;

namespace FurnitureWeb.WebApp.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
