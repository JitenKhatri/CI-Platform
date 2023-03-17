using CI_Platform.DataAccess.Repository;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
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
            VolunteeringMissionVM mission = db.MissionRepository.GetMissionById(id, long.Parse(@User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value));
            return View(mission);
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
               var InvitedMissionLink = Url.Action("volunteering_mission", "Mission", new { id = Mission_id }, Request.Scheme);

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
           
            //return Json(new { comments, success = true });
        }

        //public IActionResult Volunteering_mission()
        //{
        //    return View();
        //}
    }
}
