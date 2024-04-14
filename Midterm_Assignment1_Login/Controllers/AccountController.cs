using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using YourApp.Models;

namespace YourApp.Controllers
{
    public class AccountController : Controller
    {
        private static Dictionary<string, string> users = new Dictionary<string, string>();
        private const int MaxLoginAttempts = 3;

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (users.ContainsKey(registerModel.Username))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(registerModel);
                }

                // Store username, email, and hashed password in the users dictionary
                users.Add(registerModel.Username, registerModel.Email);
                users.Add($"{registerModel.Username}_Password", HashPassword(registerModel.Password));

                // Optionally, you can save the email to a database or another storage mechanism

                return RedirectToAction("Login");
            }
            return View(registerModel);
        }

        public ActionResult Login()
        {
            int remainingAttempts = GetRemainingAttempts();
            ViewBag.RemainingAttempts = remainingAttempts;

            // Determine whether to disable the input fields
            bool disableLoginInput = remainingAttempts == 0;
            ViewBag.DisableLoginInput = disableLoginInput;

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists and the password is correct
                if (users.ContainsKey(loginModel.Username) && BCrypt.Net.BCrypt.Verify(loginModel.Password, users[$"{loginModel.Username}_Password"]))
                {
                    // Successful login, redirect to the dashboard
                    ResetLoginAttempts();
                    return RedirectToAction("Dashboard", new { username = loginModel.Username });
                }
                else
                {
                    // Invalid username or password
                    int remainingAttempts = DecrementAndGetRemainingAttempts();
                    if (remainingAttempts == 0)
                    {
                        ResetLoginAttempts();
                        ModelState.AddModelError("", "You have exceeded the maximum number of login attempts. Please re-register.");
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Invalid account. {remainingAttempts} attempt{(remainingAttempts > 1 ? "s" : "")} remaining.");
                    }
                    // Pass remaining attempts to the view
                    ViewBag.RemainingAttempts = remainingAttempts;
                    return View(loginModel);
                }
            }
            else
            {
                // If the model state is invalid, return to the login view with validation errors
                return View(loginModel);
            }
        }

        private void ResetLoginAttempts()
        {
            HttpContext.Session.SetInt32("LoginAttempts", 0);
        }

        private int DecrementAndGetRemainingAttempts()
        {
            var attempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
            HttpContext.Session.SetInt32("LoginAttempts", attempts + 1);
            return MaxLoginAttempts - attempts;
        }

        private int GetRemainingAttempts()
        {
            var attempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
            return MaxLoginAttempts - attempts;
        }

        public ActionResult Dashboard(string username)
        {
            // Retrieve user information based on the username
            if (users.ContainsKey(username))
            {
                string email = users[username]; // Retrieve the actual email from the dictionary
                string hashedPassword = users[$"{username}_Password"]; // Retrieve the hashed password

                ViewBag.Username = username;
                ViewBag.Email = email;
                ViewBag.HashedPassword = hashedPassword; // Pass the hashed password to the view
            }
            else
            {
                // Handle the case where the username is not found
                ViewBag.Username = "Unknown";
                ViewBag.Email = "Unknown";
                ViewBag.HashedPassword = "Unknown";
            }
            return View();
        }

        public string HashPassword(string password)
        {
            // Generate a bcrypt hash with a cost factor of 12
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }
    }
}