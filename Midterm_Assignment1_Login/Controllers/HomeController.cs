using Microsoft.AspNetCore.Mvc;

namespace Midterm_Assignment1_Login.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}