using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Data;
using Dapper;

namespace CI_Platform.DataAccess.Repository
{
    public class MissionRepository : Repository<Mission>, IMissionRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MissionRepository(CiPlatformContext db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public (List<MissionViewModel>, int) GetAllMission(int page = 1, int pageSize = 3)
        {
            int skipCount = (page - 1) * pageSize;
            var ratings = _db.MissionRatings.Select(missonrating => decimal.Parse(missonrating.Rating));
            IQueryable<MissionViewModel> query = _db.Missions.Where(Mission => Mission.DeletedAt == null)
                        .Select(mission => new MissionViewModel
                        {
                            image = mission.MissionMedia.FirstOrDefault(),
                            Missions = mission,
                            Country = new CountryViewModel
                            {
                                CountryId = mission.Country.CountryId,
                                CountryName = mission.Country.Name
                            },
                            Theme = new ThemeViewModel
                            {
                                ThemeId = mission.Theme.MissionThemeId,
                                ThemeName = mission.Theme.Title
                            },
                            Skill = new SkillViewModel
                            {
                                SkillId = mission.MissionSkills.Select(ms => ms.Skill.SkillId).FirstOrDefault(),
                                SkillName = mission.MissionSkills.Select(ms => ms.Skill.SkillName).FirstOrDefault()
                            },
                            City = new CityViewModel
                            {
                                CityId = mission.CityId,
                                CityName = mission.City.Name
                            },
                            Avg_ratings = mission.MissionRatings.Select(missionrating => decimal.Parse(missionrating.Rating))
                        }); ;


            List<MissionViewModel> missions = query.Skip(skipCount).Take(pageSize).ToList();
            int totalmissions = query.Count();

            return (missions, totalmissions);
        }

        public (List<MissionViewModel>, int) GetFilteredMissions(MissionInputModel model)
        {
            int skipCount = (model.Page - 1) * model.PageSize;

            // Create a queryable object for the missions table
            IQueryable<Mission> missions = _db.Missions.Where(Mission => Mission.DeletedAt == null).AsQueryable();

            // Apply sorting based on the sortOrder parameter
            switch (model.SortOrder)
            {
                case null:
                case "Oldest":
                    missions = missions.OrderBy(mission => mission.CreatedAt);
                    break;
                case "Newest":
                    missions = missions.OrderByDescending(mission => mission.CreatedAt);
                    break;
                case "Seats_ascending":
                    missions = missions.OrderBy(mission => mission.SeatsLeft);
                    break;
                case "Seats_descending":
                    missions = missions.OrderByDescending(mission => mission.SeatsLeft);
                    break;
                case "deadline":
                    missions = missions.OrderBy(mission => mission.Deadline);
                    break;
                case "Random":
                    missions = missions.OrderBy(mission => Guid.NewGuid());
                    break;
                case "TopFavorites":
                    missions = missions
                    .GroupJoin(_db.FavoriteMissions, mission => mission.MissionId,
                    favorite => favorite.MissionId, (mission, favorites) => new { Mission = mission, FavoriteMissions = favorites })
                    .Select(group => new { Mission = group.Mission, FavoriteCount = group.FavoriteMissions.Count() })
                    .OrderByDescending(mission => mission.FavoriteCount)
                    .Select(mission => mission.Mission);
                    break;
                case "MostRanked":
                    missions = missions
                               .Select(mission => new
                               {
                                   Mission = mission,
                                   AverageRating = _db.MissionRatings
                                       .Where(rating => rating.MissionId == mission.MissionId && rating.Rating != null)
                                       .Select(rating => Convert.ToDouble(rating.Rating))
                                       .DefaultIfEmpty()
                                       .Average()
                               })
                               .OrderByDescending(mission => mission.AverageRating)
                               .Select(mission => mission.Mission);
                    break;
                case "TopThemes":
                    var topThemes = _db.MissionThemes
                               .OrderByDescending(theme => _db.Missions.Count(m => m.ThemeId == theme.MissionThemeId))
                               .Take(10);
                    missions = missions
                               .Where(mission => topThemes.Any(theme => theme.MissionThemeId == mission.ThemeId))
                        .OrderByDescending(mission => _db.Missions.Count(m => m.ThemeId == mission.ThemeId))
                        .ThenBy(mission => mission.MissionId);
     

                    break;


                default:
                    missions = missions.OrderBy(mission => mission.CreatedAt);
                    break;
            }

            // Apply filtering based on the input parameters

            missions = missions.Where(mission =>
                    (model.Cities.Count == 0 || model.Cities.Contains(mission.City.Name)) &&
                    (model.Countries.Count == 0 || model.Countries.Contains(mission.Country.Name)) &&
                    (model.Themes.Count == 0 || model.Themes.Contains(mission.Theme.Title)));

            if (model.Skills.Count > 0)
            {
                var missionIds = _db.MissionSkills
                    .Where(missionskill => model.Skills.Contains(missionskill.Skill.SkillName))
                    .Select(missionskill => missionskill.MissionId)
                    .Distinct();
                missions = missions.Where(mission => missionIds.Contains(mission.MissionId));
            }
            if (!String.IsNullOrEmpty(model.SearchText))
            {
                missions = missions.Where(mission =>
                    mission.Title.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", "")) ||
                    mission.Description.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", "")));
            }
            int totalmissions = missions.Count();
            // Create a projection of the required data
            var Missions = missions.Skip(skipCount).Take(model.PageSize)
                                   .Select(mission => new MissionViewModel
                                   {
                                       image = mission.MissionMedia.FirstOrDefault(),
                                       Missions = mission,
                                       Country = new CountryViewModel
                                       {
                                           CountryId = mission.Country.CountryId,
                                           CountryName = mission.Country.Name
                                       },
                                       City = new CityViewModel
                                       {
                                           CityId = mission.City.CityId,
                                           CityName = mission.City.Name
                                       },
                                       Theme = new ThemeViewModel
                                       {
                                           ThemeId = mission.Theme.MissionThemeId,
                                           ThemeName = mission.Theme.Title
                                       },
                                       Avg_ratings = mission.MissionRatings.Select(missionrating => decimal.Parse(missionrating.Rating))
                                   }).ToList();
            return (Missions, totalmissions);
        }



        public List<City> GetCitiesForCountry(long countryid)
        {
            if (countryid != 0)
            {
                var cities = _db.Cities
                 .Where(city => city.CountryId == countryid)
                 .ToList();

                return cities;
            }
            else
            {
                var Cities = _db.Cities.ToList();
                return Cities;
            }
        }



        public IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment)
        {

            List<User> users = _db.Users.ToList();
            Comment mycomment = new()
            {
                UserId = user_id,
                MissionId = mission_id,
                CommentText = comment
            };
            _db.Comments.Add(mycomment);
            Save();
           List<Comment> comments = _db.Comments.Where(comment => comment.MissionId == mission_id && (comment.ApprovalStatus == "PUBLISHED" || comment.UserId == user_id)).OrderByDescending(comment => comment.CreatedAt).Take(5).ToList();
            IEnumerable<CommentViewModel> mission_comments = (from Comment in comments
                                                              select new CommentViewModel { User_Comment = Comment, user = Comment.User });
            return mission_comments;
        }

        public bool add_to_favourite(long user_id, long mission_id)
        {
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            if (user_id != 0 && mission_id != 0)
            {
                var favouritemission = (from favoritemission in favoriteMissions
                                        where favoritemission.UserId.Equals(user_id) && favoritemission.MissionId.Equals(mission_id)
                                        select favoritemission).ToList();
                if (favouritemission.Count == 0)
                {
                    _db.FavoriteMissions.Add(new FavoriteMission
                    {
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    Save();
                    return true;
                }
                else
                {
                    _db.Remove(favouritemission.ElementAt(0));
                    Save();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Rate_mission(long user_id, long mission_id, int rating)
        {
            List<MissionRating> ratings = _db.MissionRatings.ToList();

            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(mission_id)
                          select r).ToList();
            if (Rating.Count == 0)
            {
                _db.MissionRatings.Add(new MissionRating
                {
                    UserId = user_id,
                    MissionId = mission_id,
                    Rating = rating.ToString()
                });
                Save();
                return true;
            }
            else
            {
                Rating.ElementAt(0).Rating = rating.ToString();
                Save();
                return true;
            }

        }
        public bool apply_for_mission(long user_id, long mission_id)
        {
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            Mission Applied_mission = _db.Missions.FirstOrDefault(mission => mission.MissionId == mission_id);
            DateTime current = DateTime.Now;
            if (user_id != 0 && mission_id != 0)
            {
                var missionapplication = (from MissionApplication in missionApplications
                                          where MissionApplication.UserId.Equals(user_id) && MissionApplication.MissionId.Equals(mission_id)
                                          select MissionApplication).ToList();
                if (missionapplication.Count == 0)
                {
                    _db.MissionApplications.Add(new MissionApplication
                    {
                        AppliedAt = current,
                        UserId = user_id,
                        MissionId = mission_id
                    });
                    if (Applied_mission.SeatsLeft != null)
                    {
                        Applied_mission.SeatsLeft = Applied_mission.SeatsLeft - 1;
                    }
                    Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public VolunteeringMissionVM Next_Volunteers(int count, long user_id, long mission_id)
        {
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            List<User> Users = _db.Users.ToList();
            Mission? mission = _db.Missions.Find(mission_id);
            List<User> users = (from MissionApplication in missionApplications
                                where MissionApplication.MissionId.Equals(mission?.MissionId) && !MissionApplication.UserId.Equals(user_id)
                                select MissionApplication.User).ToList();
            return new VolunteeringMissionVM { Missions = mission, Recent_volunteers = users.Skip(1 * count).Take(1).ToList(), Total_volunteers = users.Count };
        }
        public bool Recommend(long user_id, long mission_id, List<long> co_workers)
        {
            foreach (var user in co_workers)
            {
                _db.MissionInvites.Add(new MissionInvite
                {
                    FromUserId = user_id,
                    ToUserId = user,
                    MissionId = mission_id
                });
                User recommenduser = _db.Users.FirstOrDefault(user => user.UserId == user_id);
                Mission recommendmission = _db.Missions.FirstOrDefault(mission => mission.MissionId == mission_id);
                _db.Notifications.Add(new Notification
                {
                    MissionId = mission_id,
                    Message = recommenduser.FirstName + " " + recommenduser.LastName + " " + "Recommends this mission - " + recommendmission.Title,
                    UserId = user,
                    Status = "NOT SEEN",
                    NotificationSettingId = 1,
                    UserAvatar = recommenduser.Avatar
                });
            }
            Save();
            return true;
        }
        public VolunteeringMissionVM GetMissionById(int id, long user_id)
        {
            List<MissionRating> ratings = _db.MissionRatings.ToList();
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            List<Mission> missions = _db.Missions.Where(Mission => Mission.DeletedAt == null).ToList();
            List<MissionTheme> themes = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<MissionMedium> images = _db.MissionMedia.ToList();
            List<MissionDocument> documents = _db.MissionDocuments.ToList();
            List<User> users = _db.Users.ToList();
            List<Mission> related_mission = _db.Missions.Where(Mission => Mission.DeletedAt == null).ToList();
            List<MissionApplication> missionApplications = _db.MissionApplications.ToList();
            List<User> all_volunteers = _db.Users.Where(user => user.DeletedAt == null).ToList();
            List<User> already_recommended = new();
            List<MissionInvite> already_recommended_users = new();

            decimal avg_ratings = 0;
            int rating_count = 0;
            int rating = 0;
            var Rating = (from r in ratings
                          where r.UserId.Equals(user_id) && r.MissionId.Equals(id)
                          select r).ToList();
            bool applied_or_not = false;
            string approval_status = string.Empty;
            if (Rating.Count > 0)
            {
                rating = int.Parse(Rating.ElementAt(0).Rating);
            }



            Mission mission = _db.Missions.FirstOrDefault(mission => mission.MissionId == id);
            if (mission == null)
            {
                return null; // or throw an exception if desired
            }

            MissionTheme theme = _db.MissionThemes.FirstOrDefault(missiontheme => missiontheme.MissionThemeId == mission.ThemeId);
            List<Skill> skills = _db.MissionSkills.Where(missionskill => missionskill.MissionId == id).Select(missionskill => missionskill.Skill).ToList();
            List<Comment> comments = _db.Comments.Where(comment => comment.MissionId == id && (comment.ApprovalStatus == "PUBLISHED" || comment.UserId == user_id)).OrderByDescending(comment => comment.CreatedAt).Take(5).ToList();
            List<MissionDocument> missiondocuments = _db.MissionDocuments.Where(missiondocument => missiondocument.MissionId == id).ToList();
            City city = _db.Cities.FirstOrDefault(city => city.CityId == mission.CityId);

            if (mission.MissionRatings.Count > 0)
            {
                avg_ratings = (from m in mission.MissionRatings
                               select decimal.Parse(m.Rating)).Average();
                rating_count = (from m in mission.MissionRatings
                                select m).ToList().Count;
            }
            List<User> volunteers = (from MissionApplication in missionApplications
                                     where MissionApplication.MissionId.Equals(mission?.MissionId) && !MissionApplication.UserId.Equals(user_id)
                                     select MissionApplication.User).ToList();



            related_mission = (from Mission in missions
                               where !Mission.MissionId.Equals(mission.MissionId) && Mission.City?.Name != null && Mission.City.Name.Equals(mission.City.Name)
                               select Mission).Take(3).ToList();
            if (related_mission.Count == 0)
            {
                related_mission = (from Mission in missions
                                   where !Mission.MissionId.Equals(mission.MissionId) && Mission.Country?.Name != null && Mission.Country.Name.Equals(mission.Country.Name)
                                   select Mission).Take(3).ToList();
                if (related_mission.Count == 0)
                {
                    related_mission = (from Mission in missions
                                       where !Mission.MissionId.Equals(mission.MissionId) && Mission.Theme?.Title != null && Mission.Theme.Title.Equals(mission.Theme.Title)
                                       select Mission).Take(3).ToList();

                }
                else
                {
                    related_mission = null;
                }
            }
            List<MissionApplication> applied = (from MissionApplication in missionApplications
                                                where MissionApplication.MissionId.Equals(mission?.MissionId) && MissionApplication.UserId.Equals(user_id)
                                                select MissionApplication).ToList();
            if (applied.Count > 0)
            {
                applied_or_not = true;
                approval_status = applied.FirstOrDefault().ApprovalStatus;
            }
            var favouritemission = (from FavoriteMission in favoriteMissions
                                    where FavoriteMission.UserId.Equals(user_id) && FavoriteMission.MissionId.Equals(id)
                                    select FavoriteMission).ToList();


            return new VolunteeringMissionVM
            {
                Missions = mission,
                theme = theme,
                skills = skills,
                Cities = city,
                comments = comments,
                Rating = rating,
                Favorite_mission = favouritemission.Count,
                Avg_ratings = avg_ratings,
                Rating_count = rating_count,
                relatedMissions = related_mission,
                Applied_or_not = applied_or_not,
                ApprovalStatus = approval_status,
                Recent_volunteers = volunteers.Take(1).ToList(),
                Total_volunteers = volunteers.Count,
                documents = missiondocuments,
                All_volunteers = all_volunteers,
                MissionMedia = _db.MissionMedia.Where(missionmedium => missionmedium.MissionId == id).ToList()
            };
        }




        public void Save()
        {
            _db.SaveChanges();
        }

        // volunteering timesheet
        public TimesheetViewModel Get_Mission_For_TimeSheet(long user_id)
        {
            List<Mission> missions = _db.Missions.Where(Mission => Mission.DeletedAt == null).ToList();
            var User_Timesheets = _db.Timesheets.Where(timesheet => timesheet.UserId == user_id);
            var user_mission = _db.MissionApplications
                .Where(MissionApplication => MissionApplication.UserId == user_id)
                .Select(MissionApplication => new SelectMissionViewModel
                {
                    Mission_id = MissionApplication.MissionId,
                    Title = MissionApplication.Mission.Title,
                    Mission_type = MissionApplication.Mission.MissionType,
                    Goal_object = MissionApplication.Mission.GoalMotto,
                    StartDate = MissionApplication.Mission.StartDate,
                    EndDate = MissionApplication.Mission.EndDate,
                    Goal_Achieved = MissionApplication.Mission.GoalAcheived
                });
            return new TimesheetViewModel
            {
                Missions = user_mission.ToList(),
                Timesheets = User_Timesheets.ToList()
            };
        }

        public Timesheet AddTimeSheet(long user_id, TimesheetViewModel model, string type)
        {
            List<Mission> missions = _db.Missions.ToList();
            if (type == "goal")
            {
                Timesheet timesheet = new()
                {
                    MissionId = model.Mission_id,
                    Action = model.Actions,
                    UserId = user_id,
                    DateVolunteered = DateTime.Parse(model.Volunteered_date),
                    Notes = model.Message
                };
                Mission GoalMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == model.Mission_id);
                GoalMission.GoalAcheived -= model.Actions;
                _db.Timesheets.Add(timesheet);
                Save();
                return timesheet;
            }
            else
            {
                TimeSpan hours = TimeSpan.FromHours(model.Hours);
                TimeSpan minutes = TimeSpan.FromMinutes(model.Minutes);
                TimeSpan time = hours.Add(minutes);
                Timesheet timesheet = new()
                {
                    MissionId = model.Mission_id,
                    Time = time,
                    UserId = user_id,
                    DateVolunteered = DateTime.Parse(model.Volunteered_date),
                    Notes = model.Message,
                    Status = "SUBMIT_FOR_APPROVAL"
                };
                _db.Timesheets.Add(timesheet);
                Save();
                return timesheet;
            }
        }

        public Timesheet EditTimeSheet(long timesheet_id, TimesheetViewModel model, string type)
        {
            List<Timesheet> timesheets = _db.Timesheets.ToList();
            List<Mission> missions = _db.Missions.ToList();
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(timesheet => timesheet.TimesheetId == timesheet_id);
            if (timesheet is not null)
            {
                if (type == "time-edit")
                {
                    TimeSpan hours = TimeSpan.FromHours(model.Hours);
                    TimeSpan minutes = TimeSpan.FromMinutes(model.Minutes);
                    TimeSpan time = hours.Add(minutes);
                    timesheet.Time = time;
                    timesheet.DateVolunteered = DateTime.Parse(model.Volunteered_date);
                    timesheet.Notes = model.Message;
                    Save();
                    return timesheet;
                }
                else
                {
                    Mission GoalMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == model.Mission_id);
                    GoalMission.GoalAcheived = GoalMission.GoalAcheived + timesheet.Action - model.Actions;
                    timesheet.Action = model.Actions;
                    timesheet.DateVolunteered = DateTime.Parse(model.Volunteered_date);
                    timesheet.Notes = model.Message;

                    Save();
                    return timesheet;
                }
            }
            else
            {
                return null;
            }
        }

        public bool DeleteTimesheet(long timesheet_id)
        {
            List<Timesheet> timesheets = _db.Timesheets.ToList();
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(timesheet => timesheet.TimesheetId == timesheet_id);
            if (timesheet is not null)
            {
                Mission deleteTimesheetMission = _db.Missions.FirstOrDefault(mission => mission.MissionId == timesheet.MissionId);
                if (deleteTimesheetMission.MissionType == "GO")
                {
                    deleteTimesheetMission.GoalAcheived = deleteTimesheetMission.GoalAcheived + timesheet.Action;
                }
                _db.Timesheets.Remove(timesheet);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public NotificationViewModel GetNotificationData(long userId)
        {
            //var connection = new SqlConnection("Server=PCA19\\SQL2017;Database=CI_Platform;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=tatva123;Integrated Security=False;Encrypt=False;");
            //// Create a new SqlConnection object.

            //var procedure = "GetUserNotificationData";
            //Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            //var objDetails = SqlMapper.QueryMultiple(connection,
            //procedure, param: new { userId = userId }, commandType: CommandType.StoredProcedure);
            //List<Notification> noti = objDetails.Read<Notification>().ToList();

            //List<NotificationSetting> notisetting = objDetails.Read<NotificationSetting>().ToList();
            //List<UserNotificationSetting> usernotisetting = objDetails.Read<UserNotificationSetting>().ToList();

            //var noti = objDetails.
            //var connection = new SqlConnection("Server=PCA19\\SQL2017;Database=CI_Platform;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=sa;Password=tatva123;Integrated Security=False;Encrypt=False;");
            //var parameters = new SqlParameter[]
            //{
            //new SqlParameter("@userId", userId)       //example of string value              //example of numeric value
            //};
            //var dataSet = GetDataSet(connection, "GetUserNotificationData", parameters);

            //var firstTable = dataSet?.Tables?[0];   //use as any other data table...            //var notification = _db.Notifications.FromSqlRaw("exec GetUserNotificationData @userId", UserId).ToList();
            //var UserId = new SqlParameter("@userId", userId);
            //var notificationsetting = _db.NotificationSettings.FromSqlRaw("exec GetUserNotificationData @userId", UserId).ToList();
            var notificationsettingsdata = _db.NotificationSettings.ToList();
            var usernotificationsetting = _db.UserNotificationSettings.Where(usernotificationsetting => usernotificationsetting.UserId == userId);
            var usernotification = from notifications in _db.Notifications.Where(notification => notification.UserId == userId && notification.Status == "NOT SEEN").
                                   OrderByDescending(notification => notification.CreatedAt)
                                   join usernotificationsettings in usernotificationsetting
                                   on notifications.NotificationSettingId equals usernotificationsettings.NotificationSettingId
                                   select notifications;

            int uncheckednotificationcount = usernotification.Where(notification => notification.Status == "NOT SEEN").ToList().Count();
            return new NotificationViewModel
            {
                NotificationSettings = notificationsettingsdata,
                UserNotificationSettings = usernotificationsetting.ToList(),
                Notifications = usernotification.ToList(),
                Uncheckednotificationcount = uncheckednotificationcount
            };
        }

        public bool ChangeNotificationPreferenceUser(long userId, List<int> CheckedIds)
        {
            if (CheckedIds.Count > 0)
            {
                var usernotificationsetting = _db.UserNotificationSettings.Where(usernotificationsetting => usernotificationsetting.UserId == userId).ToList();
                if (usernotificationsetting is not null)
                {
                    foreach (var setting in usernotificationsetting)
                    {
                        _db.UserNotificationSettings.Remove(setting);
                    }
                }
                foreach (var item in CheckedIds)
                {
                    UserNotificationSetting userNotificationSetting = new();
                    userNotificationSetting.UserId = userId;
                    userNotificationSetting.NotificationSettingId = item;
                    _db.UserNotificationSettings.Add(userNotificationSetting);
                }
                Save();
                return true;
            }
            else
            {
                var usernotificationsetting = _db.UserNotificationSettings.Where(usernotificationsetting => usernotificationsetting.UserId == userId).ToList();
                if (usernotificationsetting is not null)
                {
                    foreach (var setting in usernotificationsetting)
                    {
                        _db.UserNotificationSettings.Remove(setting);
                    }
                }
                Save();
                return true;
            }
        }

        public bool ReadNotification(long NotificationId, long UserId)
        {
            if (NotificationId != null && NotificationId != 0)
            {
                var notification = _db.Notifications.FirstOrDefault(Notification => Notification.NotificationId == NotificationId);
                notification.Status = "SEEN";
                Save();
                return true;
            }
            else
            {
                var usernotifications = _db.Notifications.Where(Notification => Notification.UserId == UserId).ToList();
                foreach (var notification in usernotifications)
                {
                    notification.Status = "SEEN";
                    Save();
                }
                return true;
            }

        }

        public bool SendEmail(string Receiveremail, string emailSubject,string emailBody)
        {
            var senderEmail = new MailAddress("jitenkhatri81@gmail.com", "Jiten Khatri");
            Console.WriteLine(Receiveremail);
            
            var receiverEmail = new MailAddress(Receiveremail, "Receiver");
            var password = "evat odzv mxso djdr";
 
            var body = emailBody;
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
                Subject = emailSubject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
            return true;
        }
        public DataSet GetDataSet(SqlConnection connection, string storedProcName, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(storedProcName, connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddRange(parameters);

            var result = new DataSet();
            var dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(result);

            return result;
        }
    }
}

