using CI_Platform.DataAccess.Repository.IRepository;
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
               User userExists = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(model.Email));

                if (userExists == null)
                {
                    // User does not exist, adding a model error
                    ModelState.AddModelError("", "Invalid email or password");
                    
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
        public IActionResult registration()
        {
            return View();
        }

        [HttpPost]
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
                    TempData["email"] = user.Email;
                    var senderEmail = new MailAddress("jitenkhatri81@gmail.com", "Jiten Khatri");
                    var receiverEmail = new MailAddress(user.Email, "Receiver");
                    var password = "evat odzv mxso djdr";
                    var sub = "Reset Your Password";
                    var body = "Follow this link and reset your password" +
                        "'https://localhost:7064/resetpassword'";
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

                    return RedirectToAction("ResetPassword", new { id = myuser.UserId });

                }
            }
            return View();
        }

        //[Route("resetPassword")]
        //public IActionResult resetPassword()
        //{
        //    return View();
        //}

        [Route("resetPassword/{id}")]
        public IActionResult resetPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("resetPassword/{id}")]
        public IActionResult resetPassword(ResetPasswordViewModel pass, long id)
        {
            if (ModelState.IsValid)
            {
                User myuser = db.UserAuthentication.ResetPassword(pass.Password, id);
                if (TempData["email"] != null)
                {
                    if (myuser == null)
                    {
                        ViewData["ResetPassword"] = "false";
                        return View();
                    }
                    else if (TempData["email"].Equals(myuser.Email))
                    {
                        db.save();
                        ViewData["ResetPassword"] = "true";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewData["ResetPassword"] = "false";
                        return View();
                    }
                }
                else
                {
                    ViewData["ResetPassword"] = "false";
                    return View();
                }
            }
            return View();
        }


       

    }
}
