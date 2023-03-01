﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;

namespace CI_Platform.Controllers
{
    
    [Route("UserAuthentication")]
    public class UserAuthenticationController : Controller
    {
        private readonly IAllRepository db;
        public UserAuthenticationController(IAllRepository _db)
        {
            db = _db;
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
               User userExists = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) &&
    c.Password.Equals(model.Password));

                if (userExists == null)
                {
                    // User does not exist, adding a model error
                    ViewBag.LoginErrorMessage = "Invalid email or password";

                }
                else
                {
                    // User exists
                    return RedirectToAction("Index", "Home");
                }
            }

            // ModelState is invalid, or user does not exist, return to the login view with errors
            return View(model);
        }
        [Route("registration")]
        public IActionResult registration()
        {
            return View();
        }



        [HttpPost]
        [Route("registration")]
        public  IActionResult registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User userExists = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()));

                if (userExists == null)
                {
                    // User does not exists
                    var user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Password = model.Password
                    };
                    db.UserAuthentication.Add(user);
                    db.save();
                    TempData["SuccessMessage"] = "Registration successful. Please login to continue.";
                    return RedirectToAction("login");
                }
                else
                {
                    return BadRequest("User with specified credentials already exists");
                }

               

               
            }

            return View(model);
        }


        [Route("lostPassword")]
        public IActionResult lostPassword()
        {
           return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Route("lostPassword", Name = "UserLostPassword_validation")]
        [HttpPost]
        public IActionResult lostPassword(ForgotPasswordViewModel user)

        {
            if (ModelState.IsValid)
            {
                User myuser = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(user.Email.ToLower()));
                if (myuser == null)
                {
                    return View();
                }
                else
                {
                    // Generate a unique token for the password reset request
                    string token = Guid.NewGuid().ToString();

                    var PasswordResetLink = Url.Action("resetpassword", "UserAuthentication", new { Email = user.Email, Token = token }, Request.Scheme);

                    // Save the token and expiration date to the PasswordReset table
                    //DateTime createdAt = DateTime.Now; // Set the token expiration to 24 hours from now
                    //PasswordReset passwordReset = new PasswordReset { Email = user.Email, Token = token,CreatedAt = createdAt};

                    var ResetPasswordInfo = new PasswordReset
                    {
                        Email = user.Email,
                        Token = token
                    };
                    db.PasswordResetRepository.Add(ResetPasswordInfo);
                    db.save();


                    TempData["email"] = user.Email;
                    var senderEmail = new MailAddress("jitenkhatri81@gmail.com", "Jiten Khatri");
                    var receiverEmail = new MailAddress(user.Email, "Receiver");
                    var password = "evat odzv mxso djdr";
                    var sub = "Reset Your Password";
                    var body = "Follow this link and reset your password " + PasswordResetLink;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }

                    //return RedirectToAction("ResetPassword", new { id = myuser.UserId });
                    //return RedirectToAction("resetPassword");
                    ViewBag.EmailSent = "Mail Is Sent Successfully, Check Your Inbox!";
                }
            }
            return View(user);
        }

        //[Route("resetPassword")]
        //public IActionResult resetPassword()
        //{
        //    return View();
        //}

        //[Route("resetPassword/{id}")]
        //public IActionResult resetPassword()
        //{
        //    return View();
        //}
        //[HttpGet]
        //[Route("resetPassword")]
        //public IActionResult ResetPassword(string token)
        //{
        //    // Retrieve the PasswordReset record for the given token
        //    PasswordReset passwordReset = db.PasswordResetRepository.GetFirstOrDefault(pr => pr.Token == token);

        //    if (passwordReset == null)
        //    {
        //        // Invalid token
        //        return BadRequest("Invalid token");
        //    }
        //    else
        //    {
        //        // Valid token, display the reset password form
        //        return View();
        //    }
        //}

        //[HttpPost]
        //[Route("resetPassword/{id}")]
        //public IActionResult resetPassword(ResetPasswordViewModel pass, long id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User myuser = db.UserAuthentication.ResetPassword(pass.Password, id);
        //        if (TempData["email"] != null)
        //        {
        //            if (myuser == null)
        //            {
        //                ViewData["ResetPassword"] = "false";
        //                return View();
        //            }
        //            else if (TempData["email"].Equals(myuser.Email))
        //            {
        //                db.save();
        //                ViewData["ResetPassword"] = "true";
        //                return RedirectToAction("Login");
        //            }
        //            else
        //            {
        //                ViewData["ResetPassword"] = "false";
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            ViewData["ResetPassword"] = "false";
        //            return View();
        //        }
        //    }
        //    return View();
        //}

        //[HttpPost]
        //[Route("resetPassword")]
        //public IActionResult resetPassword(ResetPasswordViewModel pass, long id, string token)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Verify the token and email from the database
        //        PasswordReset passwordReset = db.PasswordResetRepository.GetFirstOrDefault(pr => pr.Token == token);
        //        if (passwordReset == null || !passwordReset.Email.Equals(pass.Email.ToLower()))
        //        {
        //            ViewData["ResetPassword"] = "false";
        //            return View();
        //        }

        //        // Reset the password for the user
        //        User myuser = db.UserAuthentication.ResetPassword(pass.Password, id);
        //        if (myuser == null)
        //        {
        //            ViewData["ResetPassword"] = "false";
        //            return View();
        //        }

        //        // Delete the password reset token from the database
        //        db.PasswordResetRepository.Remove(passwordReset);
        //        db.save();

        //        ViewData["ResetPassword"] = "true";
        //        return RedirectToAction("login");
        //    }

        //    return View();
        //}

        [Route("resetPassword", Name = "UserResetPassword")]
        public IActionResult resetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel
            {
                Email = email,
                token = token
            });
        }



        //reset passord
        [HttpPost]
        [Route("resetPassword")]
        public IActionResult resetPassword(ResetPasswordViewModel model)
        {
            CiPlatformContext context = new CiPlatformContext();

            //var ResetPasswordData = context.PasswordResets.Any(e => e.Email == model.Email && e.Token == model.token);
            var ResetPasswordData = db.PasswordResetRepository.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) && c.Token == model.token);


            if (ResetPasswordData!=null)
            {
                var x = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()));
                


                x.Password = model.Password;

                Console.WriteLine(x.Password);

                context.Users.Update(x);
                context.SaveChanges();
                ViewBag.PassChange = "Password Changed Successfully!";
            }
            else
            {
                ModelState.AddModelError("Token", "Reset Passwordword Link is Invalid");
            }
            return View(model);
        }

    }
}
