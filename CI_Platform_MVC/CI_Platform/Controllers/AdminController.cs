﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork db;
        public AdminController(IUnitOfWork _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var users = db.AdminRepository.GetAllUsers();
            return View(users);
        }


        [HttpPost]
        public IActionResult AddUserPartial(int countryId, int UserId)
        {
            if (UserId == 0)
            {
                if (countryId == 0)
                {
                    AddUserViewModel model = new()
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
                    AddUserViewModel model = new()
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
                        UserId = (int)user.UserId,
                        Status = user.Status
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
        public IActionResult Index(AddUserViewModel model, string Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteUser(model.UserId);
                return Json(new { success });
            }
            else
            {
                if (model.UserId == 0)
                {
                    var userExists = db.AdminRepository.UserExists(model.Email);
                    if (userExists)
                    {
                        return Json(new { success = true, message = "Email exists" });
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            bool success = db.AdminRepository.AddUser(model);
                            return Json(new { success = true });
                        }
                        else
                        {
                            return BadRequest(ModelState);
                        }
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        bool success = db.AdminRepository.AddUser(model);
                        TempData["ToastrSuccess"] = "User Edited Successfully";
                        return Json(new { success = true });
                    }
                    else
                    {
                        return BadRequest(ModelState);
                    }
                }
            }

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
                var StoryLink = Url.Action("StoryDetail", "Story", new { id = StoryId }, Request.Scheme);
                bool success = db.AdminRepository.PublishStory(StoryId, StoryLink);
                return Json(new { success = success });
            }
            else if (Action == 3)
            {
                bool success = db.AdminRepository.DeleteStory(StoryId);
                TempData["ToastrError"] = "Stored Deleted Successfully!";
                return Json(new { success = success });
            }
            else
            {
                var PolicyLink = Url.Action("Privacy", "Home", null, Request.Scheme);
                bool success = db.AdminRepository.DeclineStory(StoryId, PolicyLink);
                return Json(new { success = success });
            }
        }


        [Route("Admin/{StoryDetail}/{StoryId}")]
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
            var missionid = db.AdminRepository.GetMissionidbymissionapplicationid(MissionApplicationId);
            if (Action == 1)
            {

                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = missionid }, Request.Scheme);
                bool success = db.AdminRepository.ApproveMissionApplication(MissionApplicationId, MissionLink);
                return Json(new { success = success });
            }
            else
            {
                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = missionid }, Request.Scheme);
                bool success = db.AdminRepository.DeclineMissionApplication(MissionApplicationId, MissionLink);
                return Json(new { success = success });
            }
        }

        public IActionResult CMSCrud()
        {
            var CMSPages = db.AdminRepository.GetAllCmsPages();
            return View(CMSPages);
        }

        [HttpPost]
        public IActionResult CMSCrud(AddCMSViewModel addCMSViewModel, string Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteCMSPage(addCMSViewModel.CMSPageId);
                return Json(new { success });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    bool success = db.AdminRepository.AddCmsPage(addCMSViewModel);
                    TempData["ToastrSuccess"] = "CMS Page Saved Successfully!";
                    return Json(new { success });
                }
                else
                {
                    return BadRequest();
                }
            }

        }
        [HttpPost]
        public IActionResult AddCMSPartial(long CMSPageId)
        {
            if (CMSPageId == 0)
            {
                AddCMSViewModel addCMSViewModel = new();
                return View("_AddCMS", addCMSViewModel);
            }
            else
            {
                var CmsPage = db.AdminRepository.GetCmsPageById(CMSPageId);
                AddCMSViewModel AddCmsViewModel = new()
                {
                    CMSPageId = CmsPage.CmsPageId,
                    Title = CmsPage.Title,
                    Slug = CmsPage.Slug,
                    Description = CmsPage.Description,
                    Status = CmsPage.Status
                };
                return View("_AddCMS", AddCmsViewModel);
            }
        }

        [HttpPost]
        public IActionResult AddMissionPartial(int countryId, int MissionId)
        {
            if (MissionId == 0)
            {
                if (countryId == 0)
                {
                    AddMissionViewModel model = new()
                    {
                        Countries = db.AdminRepository.GetAllCountries(),
                        Cities = db.AdminRepository.GetAllCities(),
                        Themes = db.AdminRepository.GetAllMissionThemes(),
                        Skills = db.AdminRepository.GetAllMissionSkills()
                    };
                    return View("_AddMission", model);
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
                if (countryId == 0)
                {
                    var Mission = db.AdminRepository.GetMissionById(MissionId);
                    AddMissionViewModel model = new()
                    {
                        Countries = db.AdminRepository.GetAllCountries(),
                        Cities = db.AdminRepository.GetAllCities(),
                        Themes = db.AdminRepository.GetAllMissionThemes(),
                        Skills = db.AdminRepository.GetAllMissionSkills(),
                        MissionType = Mission.MissionType,
                        Title = Mission.Title,
                        ShortDescription = Mission.ShortDescription,
                        Description = Mission.Description,
                        Deadline = Mission.Deadline ?? new DateTime(),
                        StartDate = Mission.StartDate,
                        EndDate = Mission.EndDate,
                        CityId = Mission.CityId,
                        CountryId = Mission.CountryId,
                        SeatsLeft = Mission.SeatsLeft,
                        ThemeId = Mission.ThemeId,
                        Availability = Mission.Availability,
                        OrganizationName = Mission.OrganizationName,
                        OrganizationDetail = Mission.OrganizationDetail,
                        Goal_Motto = Mission.Goal_Motto,
                        Selected_Skills = Mission.Selected_Skills,
                        Selected_skill_names = Mission.Selected_skill_names,
                        MissionSkills = Mission.MissionSkills,
                        MissionMedia = Mission.MissionMedia,
                        MissionId = Mission.MissionId,
                        YoutubeUrl = Mission.YoutubeUrl,
                        ExistingDocuments = Mission.ExistingDocuments,
                        Goal_Achieved = Mission.Goal_Achieved
                    };
                    return View("_AddMission", model);
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
        public IActionResult MissionCrud(AddMissionViewModel model, string Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteMission(model.MissionId);
                return Json(new { success });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    long success = db.AdminRepository.AddMission(model);
                    if (success != 0)
                    {
                        var users = db.AdminRepository.GetAllUsers();
                        foreach (var user in users.Users)
                        {
                            bool isemailenabled = db.AdminRepository.NotifyuserEmail(user.UserId);
                            if (isemailenabled)
                            {
                                var email = user.Email;
                                var subject = "New Mission Added yay!!";
                                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = success }, Request.Scheme);
                                var body = "Follow this link to see the newly added mission " + MissionLink;
                                bool sentemailnotification = db.MissionRepository.SendEmail(email, subject, body);
                            }
                        }
                    }
                    return Json(new { success });
                }
                else
                {
                    return View(model);
                }
            }


        }

        public IActionResult BannerCrud()
        {
            var Banners = db.AdminRepository.GetAllBanners();
            return View(Banners);
        }

        [HttpPost]
        public IActionResult AddBannerPartial(int BannerId)
        {
            if (BannerId == 0)
            {
                AddBannerViewModel model = new();
                return View("_AddBanner", model);
            }
            else
            {
                AddBannerViewModel model = db.AdminRepository.GetBannerById(BannerId);
                return View("_AddBanner", model);
            }
        }

        [HttpPost]
        public IActionResult BannerCrud(AddBannerViewModel addBannerViewModel, string Action)
        {
            if (Action == "Delete")
            {
                bool success = db.AdminRepository.DeleteBanner(addBannerViewModel.BannerId);
                return Json(new { success });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    bool success = db.AdminRepository.Addbanner(addBannerViewModel);
                    return Json(new { success });
                }
                else
                {
                    return BadRequest();
                }
            }

        }

        public IActionResult TimesheetCrud()
        {
            var timesheets = db.AdminRepository.GetAllTimesheets();
            return View(timesheets);
        }

        [HttpPost]
        public IActionResult TimesheetCrud(int TimesheetId, int Action)
        {
            var missionid = db.AdminRepository.Getmissionidbytimesheetid(TimesheetId);
            if (Action == 1)
            {

                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = missionid }, Request.Scheme);
                bool success = db.AdminRepository.ApproveTimesheet(TimesheetId, MissionLink);
                return Json(new { success = success });
            }
            else
            {
                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = missionid }, Request.Scheme);
                bool success = db.AdminRepository.DeclineTimesheet(TimesheetId, MissionLink);
                return Json(new { success = success });
            }
        }

        public IActionResult CommentCrud()
        {
            var comments = db.AdminRepository.GetAllComments();
            return View(comments);
        }

        [HttpPost]
        public IActionResult CommentCrud(int CommentId, int Action, int MissionId)
        {
            if (Action == 1)
            {

                var MissionLink = Url.Action("volunteering_mission", "Mission", new { id = MissionId }, Request.Scheme);
                bool success = db.AdminRepository.PublishComment(CommentId, MissionLink);
                return Json(new { success = success });
            }
            else
            {
                var PolicyLink = Url.Action("Privacy", "Home", null, Request.Scheme);
                bool success = db.AdminRepository.DeclineComment(CommentId, PolicyLink);
                return Json(new { success = success });
            }
        }

    }
}







