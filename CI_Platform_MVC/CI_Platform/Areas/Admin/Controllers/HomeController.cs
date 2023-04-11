using CI_Platform.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAllRepository db;
        public HomeController(IAllRepository _db)
        {
            db = _db;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            var users = db.AdminRepository.GetAllUsers();
            return View(users);
        }
    }
}
