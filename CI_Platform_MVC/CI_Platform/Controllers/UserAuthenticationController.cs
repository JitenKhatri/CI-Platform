using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.ViewModels;
using Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

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
            if(User.Identity.IsAuthenticated)
            {
                if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "user")
                {
                    return RedirectToAction("Index", "Mission");
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            
             else {
                    ViewBag.banners = db.AdminRepository.GetBanners();
                    return View(); }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult>  login(LoginViewModel model)
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
            if (ModelState.IsValid)
            {
                // Check if a user with the provided email and password exists in the database
               User userExists = db.UserAuthentication.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) &&
    c.Password.Equals(model.Password) && c.DeletedAt == null);

                if (userExists == null)
                {
                    // User does not exist, adding a model error
                    ViewBag.LoginErrorMessage = "Invalid email or password";

                }
                else
                {
                    var claims = new List<Claim>
                            {
                                  new Claim(ClaimTypes.Name, $"{userExists.FirstName} {userExists.LastName}"),
                                  new Claim(ClaimTypes.Email, userExists.Email),
                                  new Claim(ClaimTypes.Sid, userExists.UserId.ToString()),
                                  new Claim(ClaimTypes.Role, userExists.Role.TrimEnd()),
                                  new Claim("Avatar",userExists.Avatar)
                               };
                    var identity = new ClaimsIdentity(claims, "AuthCookie");
                    var Principle = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("AuthCookie", Principle);

                    var storyid = HttpContext.Session.GetString("Storyid");
                    var missionid = HttpContext.Session.GetString("Missionid");
                    if (!string.IsNullOrEmpty(missionid))
                    {
                        // Clear the session so the story ID doesn't get used again
                        HttpContext.Session.Remove("Missionid");
                        // Redirect to the storydetail page
                        return RedirectToAction("Volunteering_mission", "Mission", new { id = long.Parse(missionid) });
                    }
                    else if (!string.IsNullOrEmpty(storyid))
                    {
                        // Clear the session so the story ID doesn't get used again
                        HttpContext.Session.Remove("Storyid");
                        // Redirect to the storydetail page
                        return RedirectToAction("StoryDetail", "Story", new { id = long.Parse(storyid) });
                    }
                    else if(Principle.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value == "admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Mission");
                    }
                    
                }
            }

            // ModelState is invalid, or user does not exist, return to the login view with errors
            return View(model);
        }
        [Route("registration")]
        public IActionResult registration()
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
            return View();
        }


        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");

            // Redirect to the home page or login page
            return RedirectToAction("login", "UserAuthentication");
        }

        [HttpPost]
        [Route("registration")]
        public  IActionResult registration(RegistrationViewModel model)
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
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
                        Password = model.Password,
                        Role = "user",
                        Avatar = "/images/user-img.png"
                    };
                    db.UserAuthentication.Add(user);
                    db.save();
                    return RedirectToAction("login");
                }
                else
                {
                    return Json(new { success = true, message = "Email exists" });
                }   
            }
            else
            {
                return BadRequest();
            }

            
        }


        [Route("lostPassword")]
        public IActionResult lostPassword()
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
            return View();
        }

        [Route("lostPassword", Name = "UserLostPassword_validation")]
        [HttpPost]
        public IActionResult lostPassword(ForgotPasswordViewModel user)

        {
            ViewBag.banners = db.AdminRepository.GetBanners();
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
                    ViewBag.EmailSent = "Mail Is Sent Successfully, Check Your Inbox!";
                }
            }
            return View(user);
        }

        [Route("resetPassword", Name = "UserResetPassword")]
        public IActionResult resetPassword(string email, string token)
        {
            ViewBag.banners = db.AdminRepository.GetBanners();
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
            ViewBag.banners = db.AdminRepository.GetBanners();
            CiPlatformContext context = new CiPlatformContext();
            if (ModelState.IsValid)
            {
                //var ResetPasswordData = context.PasswordResets.Any(e => e.Email == model.Email && e.Token == model.token);
                var ResetPasswordData = db.PasswordResetRepository.GetFirstOrDefault(c => c.Email.Equals(model.Email.ToLower()) && c.Token == model.token);


                if (ResetPasswordData != null)
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
            }
            return View(model);
        }

        [Route("EditProfile")]
        public IActionResult EditProfile()
        {
          long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
          var user = db.UserAuthentication.GetUser(user_id,0);
           return View(user);
        }

        [Route("EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(int country,EditProfileViewModel model)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (country!=0)
            {
                EditProfileViewModel details = db.UserAuthentication.GetUser(user_id, country);
                var cities = this.RenderViewAsync("_SelectCityPartial", details, true);
                return Json(new { cities = cities });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    bool success = db.UserAuthentication.UpdateProfile(model, user_id);
                    return RedirectToAction("EditProfile");
                }
                return BadRequest();

            }
        }
        [Route("ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(string password)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            bool success = db.UserAuthentication.ChangePassword(user_id, password);

            return Json(new { success = true });
        }

      
        [Route("ContactUs")]
        [HttpPost]
        public IActionResult ContactUs(string Subject, string Message)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            bool success = db.UserAuthentication.ContactUs(user_id, Subject, Message);

            return Json(new { success = true });
        }

    }
}
