﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace CI_Platform.Controllers
{
    
    [Route("UserAuthentication")]
    public class UserAuthenticationController : Controller
    {
         private readonly IUserRepository _userRepository;

        public UserAuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [Route("login")]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>  login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the provided email and password exists in the database
                var userExists = await _userRepository.UserExistsAsync(model.Email, model.Password);

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
                var userExists = await _userRepository.UserExistsAsync(model.Email);

                if (userExists)
                {
                    // User exists
                    return BadRequest("Use with specified email already exists");
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };

                await _userRepository.AddUserAsync(user);
                await _userRepository.SaveChangesAsync();
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

        //[HttpPost]
        //public async Task<IActionResult> lostPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // code here
        //        var user = await _accountRepository.GetUserByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            await _accountRepository.GenerateForgotPasswordTokenAsync(user);
        //        }

        //        ModelState.Clear();
        //        model.EmailSent = true;
        //    }
        //    return View(model);
        //}


        [Route("resetPassword")]
        public IActionResult resetPassword()
        {
            return View();
        }
    }
}
