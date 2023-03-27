using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CI_Platform.Controllers
{
    public class StoryController : Controller
    {
        private readonly IAllRepository db;
        public StoryController(IAllRepository _db)
        {
            db = _db;
        }
        public IActionResult Story(int page = 1, int pageSize = 6)
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
            ViewBag.SkillList = new SelectList(skills, "SkillId", "SkillName");
            if (User.Identity.IsAuthenticated)
            {
                long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                List<StoryViewModel> stories = db.StoryRepository.GetAllStories(user_id,page,pageSize);
                return View(stories);
            }
            else
            {
                return RedirectToAction("login", "UserAuthentication");
            }
        }

        [HttpPost]
        public JsonResult CityCascade(long countryid)
        {
            List<City> cities = db.StoryRepository.CityCascade(countryid);
            return Json(new { cities, success = true });
        }
        [HttpPost]
        public IActionResult Story(List<string> countries, List<string> cities, List<string> themes, List<string> skills,string searchtext=null, int page = 1, int pageSize = 6)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<StoryViewModel> stories = db.StoryRepository.GetFilteredStories(countries, cities, themes, skills,user_id,searchtext, page, pageSize);
            return PartialView("_stories", stories);
        }

        public IActionResult ShareStory()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<Mission> missions = db.StoryRepository.GetMissionApplications(user_id);
            return View(missions);
        }

        [HttpPost]
        public IActionResult ShareStory(long story_id, long Mission_id, string title, string published_date, string story_description, List<IFormFile> media, string type)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            
            bool success = db.StoryRepository.ShareStory(user_id, story_id, Mission_id, title, published_date, story_description, media, type);
            return Json(new { success = true });
        }
    }
}
