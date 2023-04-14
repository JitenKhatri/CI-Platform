using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using Controllers;
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
        [HttpPost]
        public IActionResult MissionThemeCrud(int ThemeId,String ThemeName,int Status,String Action)
        {
            if(Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteTheme(ThemeId);
                return Json(new { success });
            }
            else if(Action == "Edit"){
                MissionTheme EditedTheme = db.AdminRepository.EditTheme(ThemeId, Status, ThemeName);
                var view = this.RenderViewAsync("_MissionTheme", EditedTheme, true);
                return Json(new { view });
            }
            else {
                MissionTheme NewTheme = db.AdminRepository.AddTheme(ThemeName, Status);
                var view = this.RenderViewAsync("_MissionTheme", NewTheme, true);
                return Json(new { view });
            }
            
        }

        [Area("Admin")]
        public IActionResult MissionSkillCrud()
        {
            var skills = db.AdminRepository.GetAllSkills();
            return View(skills);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult MissionSkillCrud(int SkillId, String SkillName, int Status, String Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteSkill(SkillId);
                return Json(new { success });
            }
            else if (Action == "Edit")
            {
                Skill EditedSkill = db.AdminRepository.EditSkill(SkillId, Status, SkillName);
                var view = this.RenderViewAsync("_MissionSkill", EditedSkill, true);
                return Json(new { view });
            }
            else
            {
                Skill NewSkill = db.AdminRepository.AddSkill(SkillName, Status);
                var view = this.RenderViewAsync("_MissionSkill", NewSkill, true);
                return Json(new { view });
            }
        }

        [Area("Admin")]
        public IActionResult StoryCrud()
        {
            var stories = db.AdminRepository.GetAllStories();
            return View(stories);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult StoryCrud(int StoryId,int Action)
        {
            if (Action == 1)
            {
                bool success = db.AdminRepository.PublishStory(StoryId);
                return Json(new { success = success });
            }
            else
            {
                bool success = db.AdminRepository.DeclineStory(StoryId);
                return Json(new { success = success });
            }
        }

        [Area("Admin")]
        public IActionResult MissionApplications()
        {
            var missionapplications = db.AdminRepository.GetAllMissionApplications();
            return View(missionapplications);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult MissionApplications(int MissionApplicationId,int Action)
        {
           if(Action == 1)
            {
                bool success = db.AdminRepository.ApproveMissionApplication(MissionApplicationId);
                return Json(new { success = success });
            }
           else
            {
                bool success = db.AdminRepository.DeclineMissionApplication(MissionApplicationId);
                return Json(new { success = success });
            }
        }
    }
}
