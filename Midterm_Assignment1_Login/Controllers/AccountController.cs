using Microsoft.AspNetCore.Mvc;
using Midterm_Assignment1_Login.Models;
using System.Collections.Generic;
using System.Linq;

namespace Midterm_Assignment1_Login.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>();

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User model)
        {
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user != null)
            {
                // Successful login
                // Redirect to home after successful login
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Invalid credentials
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User model)
        {
            users.Add(model);
            return RedirectToAction("Login");
        }
    }
}