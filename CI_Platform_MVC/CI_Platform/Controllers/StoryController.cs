using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                List<StoryViewModel> stories = db.StoryRepository.GetAllStories(page, pageSize);
                return View(stories);
            }
            else
            {
                return RedirectToAction("login", "UserAuthentication");
            }
        }
        [HttpPost]
        public JsonResult GetCitiesForCountry(long countryid)
        {
            List<City> cities = db.StoryRepository.GetCitiesForCountry(countryid);
            return Json(new { cities, success = true });
        }
        [HttpPost]
        public IActionResult Story(List<string> countries, List<string> cities, List<string> themes, List<string> skills, int page = 1, int pageSize = 6)
        {
            List<StoryViewModel> stories = db.StoryRepository.GetFilteredStories(countries, cities, themes, skills, page, pageSize);
            return PartialView("_stories", stories);
        }
    }
}
