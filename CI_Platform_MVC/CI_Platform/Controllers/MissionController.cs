using CI_Platform.DataAccess.Repository;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CI_Platform.Controllers
{
    public class MissionController : Controller
    {
        private readonly IAllRepository db;
        public MissionController(IAllRepository _db)
        {
            db = _db;
        }

     
        public IActionResult Index()
        {
            List<City> cities = new List<City>();
            List<Skill> skills = new List<Skill>();
            List<Country> countries = new List<Country>();
            List<MissionTheme> themes = new List<MissionTheme>();

            using (var dbContext = new CiPlatformContext())
            {
                cities = dbContext.Cities.ToList();
                countries = dbContext.Countries.ToList();
                themes = dbContext.MissionThemes.ToList();
                skills = dbContext.Skills.ToList();
            }
            ViewBag.CityList = new SelectList(cities, "CityId", "Name");
            ViewBag.CountryList = new SelectList(countries, "CountryId", "Name");
            ViewBag.ThemeList = new SelectList(themes, "MissionThemeId", "Title");
            ViewBag.SkillList = new  SelectList(skills, "SkillId", "SkillName");
            

            List<MissionViewModel> missions = db.MissionRepository.GetAllMission();
            return View(missions);
        }
        [HttpPost]
        public JsonResult Index(List<string> countries, List<string> cities, List<string> themes, List<string> skills)
        {
            List<MissionViewModel> missions = db.MissionRepository.GetFilteredMissions(countries, cities, themes, skills);
            return Json(new { missions, success = true });
        }
    }
}
