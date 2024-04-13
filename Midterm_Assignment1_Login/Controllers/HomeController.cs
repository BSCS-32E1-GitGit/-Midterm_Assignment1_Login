using Microsoft.AspNetCore.Mvc;

namespace Midterm_Assignment1_Login.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ViewAccount(string username, string password)
        {
            return RedirectToAction("ViewAccount", "Account", new { username = username, password = password });
        }
    }
}