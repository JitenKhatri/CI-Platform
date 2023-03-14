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
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            else
            {
                ViewBag.UserName = "Evan Donohue";
            }
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
        public IActionResult Index(List<string> countries, List<string> cities, List<string> themes, List<string> skills, string? sortOrder)
        {
            List<MissionViewModel> missions = db.MissionRepository.GetFilteredMissions(countries, cities, themes, skills, sortOrder);
            return PartialView("_Mission", missions);
        }

        
        [HttpPost]
        public JsonResult GetCitiesForCountry(long countryid)
        {
            List<City> cities = db.MissionRepository.GetCitiesForCountry(countryid);
            return Json(new { cities, success = true });
        }

        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(int id)
        {
            VolunteeringMissionVM mission = db.MissionRepository.GetMissionById(id);
            return View(mission);
        }
    

        [HttpPost]
        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(long User_id, long Mission_id, string comment/*, int length*/)
        {
            IEnumerable<CommentViewModel> comments = db.MissionRepository.comment(User_id, Mission_id, comment/*, length*/);
            return Json(new { comments, success = true });
        }

        //public IActionResult Volunteering_mission()
        //{
        //    return View();
        //}
    }
}
