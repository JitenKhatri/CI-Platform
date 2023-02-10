using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class UserAuthenticationController : Controller
    {
        public IActionResult login()
        {
            return View();
        }
        //public IActionResult registration()
        //{
        //    return View();
        //}
        //public IActionResult lostPassword()
        //{
        //    return View();
        //}
        //public IActionResult resetPassword()
        //{
        //    return View();
        //}
    }
}
