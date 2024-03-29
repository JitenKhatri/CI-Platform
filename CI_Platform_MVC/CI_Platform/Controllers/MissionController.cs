﻿using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CI_Platform.Controllers
{
    [ProfileCompletionFilter]
    public class MissionController : Controller
    {
        private readonly IAllRepository db;
        public MissionController(IAllRepository _db)
        {
            db = _db;
        }

        
        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            List<City> cities = new List<City>();
            List<Skill> skills = new List<Skill>();
            List<Country> countries = new List<Country>();
            List<MissionTheme> themes = new List<MissionTheme>();
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
                var result = db.MissionRepository.GetAllMission(page, pageSize);
                List<MissionViewModel> missions = result.Item1;
                int totalitemcount = result.Item2;
                int pageCount = (int)Math.Ceiling((double)totalitemcount / pageSize);
                ViewData["PageCount"] = pageCount;
                ViewData["CurrentPage"] = page;

                return View(missions);
            }
            else
            {
                return RedirectToAction("login", "UserAuthentication");
            }
        }

        [HttpPost]
        public IActionResult Index(MissionInputModel model)
       {
            var result = db.MissionRepository.GetFilteredMissions(model);
            List<MissionViewModel> missions = result.Item1;
            int totalitemcount = result.Item2;
            int pageCount = (int)Math.Ceiling((double)totalitemcount / model.PageSize);
            ViewData["PageCount"] = pageCount;
            ViewData["CurrentPage"] = model.Page;
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
            if (User.Identity.IsAuthenticated)
            {
                VolunteeringMissionVM mission = db.MissionRepository.GetMissionById(id, long.Parse(@User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value));
                return View(mission);
            }
            else
            {
                string missionid = id.ToString();
                HttpContext.Session.SetString("Missionid", missionid);
                return RedirectToAction("login", "UserAuthentication");
            }
            
        }
    

        [HttpPost]
        [Route("volunteering_mission/{id}")]
        public IActionResult volunteering_mission(long User_id, long Mission_id, string comment, string request_for,int rating,int count, List<long> co_workers,string email)
        {
            if (request_for == "add_to_favourite")
            {
                bool success = db.MissionRepository.add_to_favourite(User_id, Mission_id);
                return Json(new { success = success });
            }
            else if (request_for == "rating")
            {
                bool success = db.MissionRepository.Rate_mission(User_id, Mission_id, rating);
                return Json(new { success = success });
            }
            else if (request_for == "mission")
            {
                bool success = db.MissionRepository.apply_for_mission(User_id, Mission_id);
                return Json(new { success = success });
            }
            else if (request_for == "next_volunteers")
            {
              VolunteeringMissionVM mission = db.MissionRepository.Next_Volunteers(count, User_id, Mission_id);
                var recent_volunteers = this.RenderViewAsync("_recent_volunteers", mission, true);
                return Json(new { recent_volunteers = recent_volunteers, Total_volunteers = mission.Total_volunteers });
            }
            else if (request_for == "recommend")
            {
                bool success = db.MissionRepository.Recommend(User_id, Mission_id, co_workers);
               var InvitedMissionLink = Url.Action("volunteering_mission", "Mission", new { id = Mission_id}, Request.Scheme);

                var senderEmail = new MailAddress("jitenkhatri81@gmail.com", "Jiten Khatri");
                Console.WriteLine(email);
                // Validate the email address
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                if (!regex.IsMatch(email))
                {
                    // Handle the invalid email address
                    return Json(new { success = false, error = "Invalid email address" });
                }
                var receiverEmail = new MailAddress(email, "Receiver");
                var password = "evat odzv mxso djdr";
                var sub = "You have been invited to a mission";
                var body = "Follow this link and Apply to the mission " + InvitedMissionLink;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
                return Json(new { success = success }); 
            }
            else
            {
                IEnumerable<CommentViewModel> comments = db.MissionRepository.comment(User_id, Mission_id, comment);
                var new_comment = this.RenderViewAsync("_Comment", comments, true);
                return Json(new { comments = new_comment, success = true });
            }
           
            
        }

        public IActionResult Volunteering_Timesheet()
        {
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            TimesheetViewModel model = db.MissionRepository.Get_Mission_For_TimeSheet(user_id);
            return View(model);
        }


        [HttpPost]
        public IActionResult Volunteering_Timesheet(TimesheetInputModel inputModel)
        {
            // access the properties of inputModel instead of the individual parameters
            long user_id = long.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (inputModel.Type == "goal")
            {
                TimesheetViewModel model = new TimesheetViewModel
                {
                    Mission_id = inputModel.Mission_id,
                    Volunteered_date = inputModel.Date,
                    Actions = inputModel.Actions,
                    Message = inputModel.Message
                };
                Timesheet timesheet = db.MissionRepository.AddTimeSheet(user_id, model, inputModel.Type);
                var view = this.RenderViewAsync("_timesheet", timesheet, true);
                return Json(new { view });
            }
            else if (inputModel.Type == "time-edit")
            {
                TimesheetViewModel model = new TimesheetViewModel
                {
                    Mission_id = inputModel.Mission_id,
                    Volunteered_date = inputModel.Date,
                    Hours = inputModel.Hours,
                    Minutes = inputModel.Minutes,
                    Message = inputModel.Message
                };
                Timesheet timesheet = db.MissionRepository.EditTimeSheet(inputModel.Timesheet_id, model, inputModel.Type);
                var view = this.RenderViewAsync("_timesheet", timesheet, true);
                return Json(new { view });
            }
            else if (inputModel.Type == "time-delete")
            {
                bool success = db.MissionRepository.DeleteTimesheet(inputModel.Timesheet_id);
                return Json(new { success });
            }
            else if (inputModel.Type == "goal-edit")
            {
                TimesheetViewModel model = new TimesheetViewModel
                {
                    Mission_id = inputModel.Mission_id,
                    Volunteered_date = inputModel.Date,
                    Actions = inputModel.Actions,
                    Message = inputModel.Message
                };
                Timesheet timesheet = db.MissionRepository.EditTimeSheet(inputModel.Timesheet_id, model, inputModel.Type);
                var view = this.RenderViewAsync("_timesheet", timesheet, true);
                return Json(new { view });
            }
            else
            {
                TimesheetViewModel model = new TimesheetViewModel
                {
                    Mission_id = inputModel.Mission_id,
                    Volunteered_date = inputModel.Date,
                    Hours = inputModel.Hours,
                    Minutes = inputModel.Minutes,
                    Message = inputModel.Message
                };
                Timesheet timesheet = db.MissionRepository.AddTimeSheet(user_id, model, inputModel.Type);
                var view = this.RenderViewAsync("_timesheet", timesheet, true);
                return Json(new { view });
            }
        }

    }

}
