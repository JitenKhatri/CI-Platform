using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CI_Platform.Controllers
{
    [ProfileCompletionFilter]
    public class StoryController : Controller
    {
        private readonly IUnitOfWork db;
        public StoryController(IUnitOfWork _db)
        {
            db = _db;
        }
        public IActionResult Story(int page = 1, int pageSize = 3)
        {
            List<City> cities = new();
            List<Skill> skills = new();
            List<Country> countries = new();
            List<MissionTheme> themes = new();
            using (var dbContext = new CiPlatformContext())
            {
                cities = dbContext.Cities.ToList();
                countries = dbContext.Countries.ToList();
                themes = dbContext.MissionThemes.Where(theme => theme.DeletedAt == null).ToList();
                skills = dbContext.Skills.Where(skill => skill.DeletedAt == null).ToList();

                ViewData["CityList"] = new SelectList(cities, "CityId", "Name");
                ViewData["CountryList"] = new SelectList(countries, "CountryId", "Name");
                ViewData["ThemeList"] = new SelectList(themes, "MissionThemeId", "Title");
                ViewData["SkillList"] = new SelectList(skills, "SkillId", "SkillName");
            }
            if (User.Identity.IsAuthenticated)
            {
                long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                var result = db.StoryRepository.GetAllStories(user_id, page, pageSize);
                List<StoryViewModel> stories = result.Item1;
                int totalItemCount = result.Item2;
                // Generate pagination links
                int pageCount = (int)Math.Ceiling((double)totalItemCount / pageSize);
                ViewData["PageCount"] = pageCount;
                ViewData["CurrentPage"] = page;
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
        public IActionResult Story(StoryInputModel model)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            model.UserId = user_id;
            var result = db.StoryRepository.GetFilteredStories(model);
            List<StoryViewModel> stories = result.Item1;
            int totalItemCount = result.Item2;
            // Generate pagination links
            int pageCount = (int)Math.Ceiling((double)totalItemCount / model.PageSize);
            ViewData["PageCount"] = pageCount;
            ViewData["CurrentPage"] = model.Page;
            return PartialView("_stories", stories);
        }

        public IActionResult ShareStory()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            List<Mission> missions = db.StoryRepository.GetMissionApplications(user_id);
            return View(missions);
        }

        [HttpPost]
        public IActionResult ShareStory(StoryInputModel model)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            model.UserId = user_id;
            bool success = db.StoryRepository.ShareStory(model);
            return Json(new { success = true });
        }

        
        public IActionResult StoryDetail(long id)
        {
            if (User.Identity.IsAuthenticated)
            {
                long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                StoryViewModel story = db.StoryRepository.GetStoryDetail(user_id, id);
                db.StoryRepository.Add_View(user_id, id);
                return View(story);
            }
            else
            {
                string storyid = id.ToString();
                HttpContext.Session.SetString("Storyid", storyid);
                return RedirectToAction("login", "UserAuthentication");
            }
         
        }

        [HttpPost]
        public IActionResult StoryDetail(long story_id,string email, long to_user_id)
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            bool success = db.StoryRepository.Recommend(user_id, story_id, to_user_id);
            var InvitedStoryLink = Url.Action("StoryDetail", "Story", new { id = story_id }, Request.Scheme);
            var body = "Follow this link and see the story " + InvitedStoryLink;
            var sub = "You have been recommended a story!!";
            bool emailsent = db.MissionRepository.SendEmail(email, sub, body);
            if (emailsent)
            {
                return Json(new { success = success });
            }
            else
            {
                return Json(new { success = false });
            }
        }

    }
}
