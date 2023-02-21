using CI_Platform.Models;
using CI_Platform.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    
    [Route("UserAuthentication")]
    public class UserAuthenticationController : Controller
    {
        private readonly DataAccess.CiPlatformContext _context;

        public UserAuthenticationController(DataAccess.CiPlatformContext context)
        {
            _context = context;
        }
        [Route("login")]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the provided email and password exists in the database
                var userExists = _context.Users.Any(u => u.Email == model.Email && u.Password == model.Password);

                if (userExists)
                {
                    // User exists
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // User does not exist, adding a model error
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            // ModelState is invalid, or user does not exist, return to the login view with errors
            return View(model);
        }
        public IActionResult registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registration successful. Please login to continue.";
                return RedirectToAction("login");
            }

            return View(model);
        }


        [Route("lostPassword")]
        public IActionResult lostPassword()
        {
           return View();
        }
        [Route("resetPassword")]
        public IActionResult resetPassword()
        {
            return View();
        }
    }
}
