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

        [Area("Admin")]
        public IActionResult MissionCrud()
        {
            var missions = db.AdminRepository.GetAllMissions();
            return View(missions);
        }

        [Area("Admin")]
        public IActionResult MissionThemeCrud()
        {
            var themes = db.AdminRepository.GetAllThemes();
            return View(themes);
        }

        [Area("Admin")]
        public IActionResult MissionSkillCrud()
        {
            var skills = db.AdminRepository.GetAllSkills();
            return View(skills);
        }


        [Area("Admin")]
        public IActionResult StoryCrud()
        {
            var stories = db.AdminRepository.GetAllStories();
            return View(stories);
        }
    }
}
