using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAllRepository db;
        public AdminController(IAllRepository _db)
        {
            db = _db;
        }
       
        public IActionResult Index()
        {
            var users = db.AdminRepository.GetAllUsers();
            return View(users);
        }

        
        [HttpPost]
        public IActionResult AddUserPartial(int countryId,int UserId)
        {
            if(UserId == 0)
            {
                if (countryId == 0)
                {
                    AddUserViewModel model = new AddUserViewModel
                    {
                        Countries = db.AdminRepository.GetAllCountries(),
                        Cities = db.AdminRepository.GetAllCities()
                    };
                    return View("_AddUser", model);
                }
                else
                {
                    AddUserViewModel model = db.AdminRepository.GetCitiesForCountries(countryId);
                    var cities = this.RenderViewAsync("_CascadeCityPartial", model, true);
                    return Json(new { cities = cities });
                }
            }
            else
            {
                var user = db.AdminRepository.GetUserById(UserId);
                if (countryId == 0)
                {
                    AddUserViewModel model = new AddUserViewModel
                    {
                        Countries = db.AdminRepository.GetAllCountries(),
                        Cities = db.AdminRepository.GetAllCities(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Department = user.Department,
                        CityId = user.CityId,
                        CountryId = user.CountryId,
                        EmployeeId = user.EmployeeId,
                        Email = user.Email,
                        Password = user.Password,
                        AvatarPath = user.Avatar,
                        ProfileText = user.ProfileText,
                        UserId = (int)user.UserId
                    };
                    return View("_AddUser", model);
                }
                else
                {
                    AddUserViewModel model = db.AdminRepository.GetCitiesForCountries(countryId);
                    var cities = this.RenderViewAsync("_CascadeCityPartial", model, true);
                    return Json(new { cities = cities });
                }
            }
            
        }

        
        [HttpPost]
        public IActionResult Index(AddUserViewModel model,string Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteUser(model.UserId);
                return Json(new { success });
            }
            else {
                var userExists = db.AdminRepository.UserExists(model.Email);
                if (userExists)
                {
                    return BadRequest();
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        bool success = db.AdminRepository.AddUser(model);
                        return Json(new { success });
                    }
                    else
                    {
                        return BadRequest();
                    }
                } }
        }
        
        public IActionResult MissionCrud()
        {
            var missions = db.AdminRepository.GetAllMissions();
            return View(missions);
        }

        
        public IActionResult MissionThemeCrud()
        {
            var themes = db.AdminRepository.GetAllThemes();
            return View(themes);
        }

        
        [HttpPost]
        public IActionResult MissionThemeCrud(int ThemeId, String ThemeName, int Status, String Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteTheme(ThemeId);
                return Json(new { success });
            }
            else if (Action == "Edit")
            {
                MissionTheme EditedTheme = db.AdminRepository.EditTheme(ThemeId, Status, ThemeName);
                var view = this.RenderViewAsync("_MissionTheme", EditedTheme, true);
                return Json(new { view });
            }
            else
            {
                MissionTheme NewTheme = db.AdminRepository.AddTheme(ThemeName, Status);
                var view = this.RenderViewAsync("_MissionTheme", NewTheme, true);
                return Json(new { view });
            }

        }

        
        public IActionResult MissionSkillCrud()
        {
            var skills = db.AdminRepository.GetAllSkills();
            return View(skills);
        }

        
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

        
        public IActionResult StoryCrud()
        {
            var stories = db.AdminRepository.GetAllStories();
            return View(stories);
        }

        
        [HttpPost]
        public IActionResult StoryCrud(int StoryId, int Action)
        {
            if (Action == 1)
            {
                bool success = db.AdminRepository.PublishStory(StoryId);
                return Json(new { success = success });
            }
            else if (Action == 3)
            {
                bool success = db.AdminRepository.DeleteStory(StoryId);
                return Json(new { success = success });
            }
            else
            {
                bool success = db.AdminRepository.DeclineStory(StoryId);
                return Json(new { success = success });
            }
        }

        
        
        public IActionResult StoryDetail(int StoryId)
        {
            StoryViewModel story = db.AdminRepository.GetStoryDetail(StoryId);
            return View(story);
        }
        
        public IActionResult MissionApplications()
        {
            var missionapplications = db.AdminRepository.GetAllMissionApplications();
            return View(missionapplications);
        }

        
        [HttpPost]
        public IActionResult MissionApplications(int MissionApplicationId, int Action)
        {
            if (Action == 1)
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

        public IActionResult CMSCrud()
        {
            var CMSPages = db.AdminRepository.GetAllCmsPages();
            return View(CMSPages);
        }

        [HttpPost]
        public IActionResult CMSCrud(AddCMSViewModel addCMSViewModel)
        {
            if (ModelState.IsValid)
            {
                bool success = db.AdminRepository.AddCmsPage(addCMSViewModel);
                return Json(new { success });
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult AddCMSPartial()
        {
            AddCMSViewModel addCMSViewModel = new AddCMSViewModel();
            return View("_AddCMS", addCMSViewModel);
        }
    }
}

