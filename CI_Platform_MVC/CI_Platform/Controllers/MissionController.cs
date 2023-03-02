using CI_Platform.DataAccess.Repository;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class MissionController : Controller
    {
        private readonly CiPlatformContext _db;
        public MissionController(CiPlatformContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Mission> missions = _db.Missions;
            return View(missions);
        }
    }
}
