using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.InputModels;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
                        .Select(m => new MissionViewModel
                        {
                            image = m.MissionMedia.FirstOrDefault(),
                            Missions = m,
                            Country = new CountryViewModel
                            {
                                CountryId = m.Country.CountryId,
                                CountryName = m.Country.Name
                            },
                            Theme = new ThemeViewModel
                            {
                                ThemeId = m.Theme.MissionThemeId,
                                ThemeName = m.Theme.Title
                            },
                            Skill = new SkillViewModel
                            {
                                SkillId = m.MissionSkills.Select(ms => ms.Skill.SkillId).FirstOrDefault(),
                                SkillName = m.MissionSkills.Select(ms => ms.Skill.SkillName).FirstOrDefault()
                            },
                            City = new CityViewModel
                            {
                                CityId = m.CityId,
                                CityName = m.City.Name
                            },
                            Avg_ratings = m.MissionRatings.Select(missionrating => decimal.Parse(missionrating.Rating))
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
                    missions = missions.OrderBy(m => m.CreatedAt);
                    break;
                case "Newest":
                    missions = missions.OrderByDescending(m => m.CreatedAt);
                    break;
                case "Seats_ascending":
                    missions = missions.OrderBy(m => m.SeatsLeft);
                    break;
                case "Seats_descending":
                    missions = missions.OrderByDescending(m => m.SeatsLeft);
                    break;
                case "deadline":
                    missions = missions.OrderBy(m => m.Deadline);
                    break;
                case "Random":
                    missions = missions.OrderBy(m => Guid.NewGuid());
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
                    missions = missions
                                .OrderByDescending(mission => _db.Missions.Count(m => m.ThemeId == mission.ThemeId))
                                .ThenBy(mission => mission.MissionId);
                    break;


                default:
                    missions = missions.OrderBy(m => m.CreatedAt);
                    break;
            }

            // Apply filtering based on the input parameters

            missions = missions.Where(m =>
                    (model.Cities.Count == 0 || model.Cities.Contains(m.City.Name)) &&
                    (model.Countries.Count == 0 || model.Countries.Contains(m.Country.Name)) &&
                    (model.Themes.Count == 0 || model.Themes.Contains(m.Theme.Title)));

            if (model.Skills.Count > 0)
            {
                var missionIds = _db.MissionSkills
                    .Where(ms => model.Skills.Contains(ms.Skill.SkillName))
                    .Select(ms => ms.MissionId)
                    .Distinct();
                missions = missions.Where(m => missionIds.Contains(m.MissionId));
            }
            if (!String.IsNullOrEmpty(model.SearchText))
            {
                missions = missions.Where(m =>
                    m.Title.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", "")) ||
                    m.Description.ToLower().Replace(" ", "").Contains(model.SearchText.ToLower().Replace(" ", "")));
            }
            int totalmissions = missions.Count();
            // Create a projection of the required data
            var Missions = missions.Skip(skipCount).Take(model.PageSize)
                                   .Select(m => new MissionViewModel
                                   {
                                       image = m.MissionMedia.FirstOrDefault(),
                                       Missions = m,
                                       Country = new CountryViewModel
                                       {
                                           CountryId = m.Country.CountryId,
                                           CountryName = m.Country.Name
                                       },
                                       City = new CityViewModel
                                       {
                                           CityId = m.City.CityId,
                                           CityName = m.City.Name
                                       },
                                       Theme = new ThemeViewModel
                                       {
                                           ThemeId = m.Theme.MissionThemeId,
                                           ThemeName = m.Theme.Title
                                       },
                                       Avg_ratings = m.MissionRatings.Select(missionrating => decimal.Parse(missionrating.Rating))
                                   }).ToList();
            return (Missions, totalmissions);
        }



        public List<City> GetCitiesForCountry(long countryid)
        {
            if (countryid != 0)
            {
                var cities = _db.Cities
                 .Where(c => c.CountryId == countryid)
                 .ToList();

                return cities;
            }
            else
            {
                var Cities = _db.Cities.ToList();
                return Cities;
            }
        }


        public List<MissionSkill> MissionSkillList(int id)
        {
            return _db.MissionSkills.Where(m => m.MissionId == id).ToList();
        }
        public IEnumerable<CommentViewModel> comment(long user_id, long mission_id, string comment)
        {

            List<Comment> comments = _db.Comments.ToList();
            List<User> users = _db.Users.ToList();
            Comment mycomment = new Comment()
            {
                UserId = user_id,
                MissionId = mission_id,
                CommentText = comment
            };
            _db.Comments.Add(mycomment);
            Save();
            comments = _db.Comments.OrderByDescending(m => m.CreatedAt).Take(5).ToList();
            IEnumerable<CommentViewModel> mission_comments = (from c in comments
                                                              where c.MissionId.Equals(mission_id)
                                                              select new CommentViewModel { User_Comment = c, user = c.User });
            return mission_comments;
        }

        public bool add_to_favourite(long user_id, long mission_id)
        {
            List<FavoriteMission> favoriteMissions = _db.FavoriteMissions.ToList();
            if (user_id != 0 && mission_id != 0)
            {
                var favouritemission = (from fm in favoriteMissions
                                        where fm.UserId.Equals(user_id) && fm.MissionId.Equals(mission_id)
                                        select fm).ToList();
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
            Mission Applied_mission = _db.Missions.FirstOrDefault(x => x.MissionId == mission_id);
            DateTime current = DateTime.Now;
            if (user_id != 0 && mission_id != 0)
            {
                var missionapplication = (from ma in missionApplications
                                          where ma.UserId.Equals(user_id) && ma.MissionId.Equals(mission_id)
                                          select ma).ToList();
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
            List<User> users = (from ma in missionApplications
                                where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                                select ma.User).ToList();
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
            List<User> already_recommended = new List<User>();
            List<MissionInvite> already_recommended_users = new List<MissionInvite>();

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



            Mission mission = _db.Missions.FirstOrDefault(m => m.MissionId == id);
            if (mission == null)
            {
                return null; // or throw an exception if desired
            }

            MissionTheme theme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == mission.ThemeId);
            List<Skill> skills = _db.MissionSkills.Where(s => s.MissionId == id).Select(s => s.Skill).ToList();
            List<Comment> comments = _db.Comments.Where(s => s.MissionId == id).OrderByDescending(m => m.CreatedAt).Take(15).ToList();
            List<MissionDocument> missiondocuments = _db.MissionDocuments.Where(ms => ms.MissionId == id).ToList();
            City city = _db.Cities.FirstOrDefault(c => c.CityId == mission.CityId);

            if (mission.MissionRatings.Count > 0)
            {
                avg_ratings = (from m in mission.MissionRatings
                               select decimal.Parse(m.Rating)).Average();
                rating_count = (from m in mission.MissionRatings
                                select m).ToList().Count;
            }
            List<User> volunteers = (from ma in missionApplications
                                     where ma.MissionId.Equals(mission?.MissionId) && !ma.UserId.Equals(user_id)
                                     select ma.User).ToList();



            related_mission = (from m in missions
                               where !m.MissionId.Equals(mission.MissionId) && m.City?.Name != null && m.City.Name.Equals(mission.City.Name)
                               select m).Take(3).ToList();
            if (related_mission.Count == 0)
            {
                related_mission = (from m in missions
                                   where !m.MissionId.Equals(mission.MissionId) && m.Country?.Name != null && m.Country.Name.Equals(mission.Country.Name)
                                   select m).Take(3).ToList();
                if (related_mission.Count == 0)
                {
                    related_mission = (from m in missions
                                       where !m.MissionId.Equals(mission.MissionId) && m.Theme?.Title != null && m.Theme.Title.Equals(mission.Theme.Title)
                                       select m).Take(3).ToList();

                }
                else
                {
                    related_mission = null;
                }
            }
            List<MissionApplication> applied = (from ma in missionApplications
                                                where ma.MissionId.Equals(mission?.MissionId) && ma.UserId.Equals(user_id)
                                                select ma).ToList();
            if (applied.Count > 0)
            {
                applied_or_not = true;
                approval_status = applied.FirstOrDefault().ApprovalStatus;
            }
            var favouritemission = (from fm in favoriteMissions
                                    where fm.UserId.Equals(user_id) && fm.MissionId.Equals(id)
                                    select fm).ToList();


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
            var User_Timesheets = _db.Timesheets.Where(amt => amt.UserId == user_id);
            var user_mission = _db.MissionApplications
                .Where(m => m.UserId == user_id)
                .Select(m => new SelectMissionViewModel
                {
                    Mission_id = m.MissionId,
                    Title = m.Mission.Title,
                    Mission_type = m.Mission.MissionType,
                    Goal_object = m.Mission.GoalMotto,
                    StartDate = m.Mission.StartDate,
                    EndDate = m.Mission.EndDate,
                    Goal_Achieved = m.Mission.GoalAcheived
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
                Timesheet timesheet = new Timesheet
                {
                    MissionId = model.Mission_id,
                    Action = model.Actions,
                    UserId = user_id,
                    DateVolunteered = DateTime.Parse(model.Volunteered_date),
                    Notes = model.Message
                };
                Mission GoalMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == model.Mission_id);
                GoalMission.GoalAcheived = GoalMission.GoalAcheived - model.Actions;
                _db.Timesheets.Add(timesheet);
                Save();
                return timesheet;
            }
            else
            {
                TimeSpan hours = TimeSpan.FromHours(model.Hours);
                TimeSpan minutes = TimeSpan.FromMinutes(model.Minutes);
                TimeSpan time = hours.Add(minutes);
                Timesheet timesheet = new Timesheet
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
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == timesheet_id);
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
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(t => t.TimesheetId == timesheet_id);
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
            var notificationsettingsdata = _db.NotificationSettings.ToList();
            var usernotificationsetting = _db.UserNotificationSettings.Where(usernotificationsetting => usernotificationsetting.UserId == userId);
            var usernotification = from notifications in _db.Notifications.Where(notification => notification.UserId == userId).
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
                    UserNotificationSetting userNotificationSetting = new UserNotificationSetting();
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
    }
}

