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
        public IActionResult Register(Registermodel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "The password and confirmation password do not match.");
                return View(model);
            }

            // Convert Registermodel to User
            User user = new User
            {
                // Assuming properties of Registermodel and User are compatible
                Username = model.Username,
                Password = model.Password,
                // Add other properties as needed
            };

            // Add the converted User object to your users collection
            users.Add(user);

            return RedirectToAction("Login");
        }
    }


}