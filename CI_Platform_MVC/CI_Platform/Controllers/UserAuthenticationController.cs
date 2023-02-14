using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    [ApiController]
    [Route("UserAuthentication")]
    public class UserAuthenticationController : Controller
    {
        [Route("login")]
        public IActionResult login()
        {
            return View();
        }
        //public IActionResult registration()
        //{
        //    return View();
        //}
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
