using Microsoft.AspNetCore.Mvc;
using Midterm_Assignment1_Login.Models;
using System.Collections.Generic;

namespace Midterm_Assignment1_Login.Controllers
{
    public class AccountController : Controller
    {
        private static Dictionary<string, User> users = new Dictionary<string, User>();
        private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (users.ContainsKey(model.Username))
            {
                var user = users[model.Username];
                if (user.Password == model.Password)
                {
                    // Successful login
                    // Redirect to account information view after successful login
                    return RedirectToAction("ViewAccount", new { username = model.Username });
                }
                else
                {
                    // Invalid credentials
                    ModelState.AddModelError("", "Invalid username or password");
                    if (!loginAttempts.ContainsKey(model.Username))
                    {
                        loginAttempts[model.Username] = 1;
                    }
                    else
                    {
                        loginAttempts[model.Username]++;
                        if (loginAttempts[model.Username] >= 3)
                        {
                            ModelState.AddModelError("", "Login attempts exceeded. Please re-register.");
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"{3 - loginAttempts[model.Username]} attempts remaining");
                        }
                    }
                    return View(model);
                }
            }
            else
            {
                // User not found
                ModelState.AddModelError("", "Invalid account");
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "The password and confirmation password do not match.");
                return View(model);
            }

            if (users.ContainsKey(model.Username))
            {
                ModelState.AddModelError("", "Username already exists. Please choose a different username.");
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
            users.Add(user.Username, user);

            return RedirectToAction("Login");
        }

        // GET: /Account/ViewAccount
        public IActionResult ViewAccount(string username, string password)
        {
            if (username != null && users.ContainsKey(username) && users[username].Password == password)
            {
                var user = users[username];
                return View(user);
            }
            else
            {
                // User not found or username/password is incorrect
                return NotFound();
            }
        }
    }
}