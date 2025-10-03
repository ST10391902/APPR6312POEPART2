using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using APPR6312PART2.Models;
using System.Collections.Generic;
using System.Linq;

namespace APPR6312PART2.Controllers
{
    public class AuthController : Controller
    {
        // Static list to store users (replace with database in real application)
        private static List<User> _users = new List<User>();
        private static int _nextUserId = 1;

        // Registration Page
        public IActionResult Register()
        {
            return View();
        }

        // Handle Registration Form Submission
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already registered");
                    return View(user);
                }

                // Assign user ID and add to list
                user.UserId = _nextUserId++;
                _users.Add(user);

                // Store user in session
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetString("UserType", user.UserType);

                TempData["SuccessMessage"] = "Registration successful! Welcome to Disaster Alleviation Foundation.";
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // Login Page
        public IActionResult Login()
        {
            return View();
        }

        // Handle Login Form Submission
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = _users.FirstOrDefault(u => u.Email == loginModel.Email && u.Password == loginModel.Password && u.IsActive);

                if (user != null)
                {
                    // Store user in session
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                    HttpContext.Session.SetString("UserType", user.UserType);

                    TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password");
            }

            return View(loginModel);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // User Profile
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Please log in to view your profile.";
                return RedirectToAction("Login");
            }

            var user = _users.FirstOrDefault(u => u.UserId.ToString() == userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // Update Profile
        [HttpPost]
        public IActionResult Profile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var existingUser = _users.FirstOrDefault(u => u.UserId.ToString() == userId);

                if (existingUser != null)
                {
                    // Update user details
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.PhoneNumber = updatedUser.PhoneNumber;
                    existingUser.UserType = updatedUser.UserType;

                    // Update session
                    HttpContext.Session.SetString("UserName", $"{updatedUser.FirstName} {updatedUser.LastName}");
                    HttpContext.Session.SetString("UserType", updatedUser.UserType);

                    TempData["SuccessMessage"] = "Profile updated successfully!";
                    return RedirectToAction("Profile");
                }
            }

            return View(updatedUser);
        }
    }
}