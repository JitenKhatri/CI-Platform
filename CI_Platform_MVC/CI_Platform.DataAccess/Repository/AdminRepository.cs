﻿using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Data.Entity;
using System.Web.Mvc;

namespace CI_Platform.DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IMissionRepository repository;
        private readonly LinkGenerator _urlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminRepository(CiPlatformContext db, IMissionRepository _repository, LinkGenerator urlHelper, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _db = db;
            repository = _repository;
            _urlHelper = urlHelper;
            _httpContextAccessor = httpContextAccessor;
        }   

        public CrudViewModel GetAllUsers()
        {
            var users = _db.Users.Where(User=>User.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                Users = users
            };
        }

        public bool DeleteUser(int UserId)
        {
            User User = _db.Users.FirstOrDefault(User => User.UserId == UserId);
            if(User !=null)
            {
                User.DeletedAt = DateTime.Now;
                //comment
                List<Comment> DeletedUserComments = _db.Comments.Where(Comment => Comment.UserId == UserId).ToList();
                foreach(var Comment in DeletedUserComments)
                {
                    Comment.DeletedAt = DateTime.Now;
                }
                //favoritemission
                List<FavoriteMission> DeletedUserFM = _db.FavoriteMissions.Where(FavoriteMission => FavoriteMission.UserId == UserId).ToList();
                foreach(var FavoriteMission in DeletedUserFM)
                {
                    FavoriteMission.DeletedAt = DateTime.Now;
                }
                //missionapplication
                List<MissionApplication> DeletedUserMA = _db.MissionApplications.Where(MissionApplication => MissionApplication.UserId == UserId).ToList();
                foreach(var MissionApplication in DeletedUserMA)
                {
                    MissionApplication.DeletedAt = DateTime.Now;
                }
                //missionrating
                List<MissionRating> DeletedUserMR = _db.MissionRatings.Where(MissionRating => MissionRating.UserId == UserId).ToList();
                foreach(var MissionRating in DeletedUserMR)
                {
                    MissionRating.DeletedAt = DateTime.Now;
                }
                //story
                List<Story> DeletedUserStories = _db.Stories.Where(Story => Story.UserId == UserId).ToList();
                foreach(var Story in DeletedUserStories)
                {
                    Story.DeletedAt = DateTime.Now;
                }
                //timesheets
                List<Timesheet> DeletedUserTS = _db.Timesheets.Where(Timesheet => Timesheet.UserId == UserId).ToList();
                foreach(var timesheet in DeletedUserTS)
                {
                    timesheet.DeletedAt = DateTime.Now;
                }
                Save();
                return true;
            }
            else
            {
                return false;
            }
           
        }
        public CrudViewModel GetAllMissions()
        {
            var missions = _db.Missions.Where(mission => mission.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                Missions = missions
            };
        }

        public CrudViewModel GetAllThemes()
        {
            var missionThemes = _db.MissionThemes.Where(missiontheme => missiontheme.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                MissionThemes = missionThemes
            };
        }
        
        public MissionTheme AddTheme(string ThemeName, int status)
        {
            MissionTheme NewTheme = new()
            {
                Title = ThemeName,
                Status = status
            };
            _db.MissionThemes.Add(NewTheme);
            Save();
            return NewTheme;
        }

        public bool DeleteTheme(int theme_id)
        {
            MissionTheme deletetheme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == theme_id );
            if (deletetheme is not null)
            {
                deletetheme.DeletedAt = DateTime.Now;
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public MissionTheme EditTheme(int theme_id,int Status,string ThemeName)
        {
            MissionTheme edittheme = _db.MissionThemes.FirstOrDefault(t => t.MissionThemeId == theme_id);
            if(edittheme is not null)
            {
                edittheme.Status = Status;
                edittheme.Title = ThemeName;
                Save();
                return edittheme;
            }
            else
            {
                return null;
            }
        }
        public CrudViewModel GetAllSkills()
        {
            List<Skill> Skills = _db.Skills.Where(skill => skill.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                Skills = Skills
            };
        }

        public List<Country> GetAllCountries()
        {
            List<Country> Countries = _db.Countries.ToList();
            return Countries;
        }

        public List<City> GetAllCities()
        {
            List<City> Cities = _db.Cities.ToList();
            return Cities;
        }

        public List<MissionTheme> GetAllMissionThemes()
        {
            List<MissionTheme> themes = _db.MissionThemes.Where(MissionTheme => MissionTheme.DeletedAt == null).ToList();
            return themes;
        }

        public List<Skill> GetAllMissionSkills()
        {
            List<Skill> skills = _db.Skills.Where(Skill => Skill.DeletedAt == null).ToList();
            return skills;
        }
        public User GetUserById(int UserId)
        {
            User user = _db.Users.FirstOrDefault(User => User.UserId == UserId);
            return user;
        }
        public AddUserViewModel GetCitiesForCountries(int CountryId)
        {
            List<City> cities = _db.Cities.Where(c => c.CountryId == CountryId).ToList();
            return new AddUserViewModel { Cities = cities };
        }

        public bool AddUser(AddUserViewModel model)
        {
            User? User = _db.Users.FirstOrDefault(c => c.UserId == model.UserId);
            if (User is null)
            {
                var user = new User();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.ProfileText = model.ProfileText;
                user.Password = model.Password;
                user.Department = model.Department;
                user.CityId = model.CityId;
                user.CountryId = model.CountryId;
                user.CreatedAt = DateTime.Now;
                user.EmployeeId = model.EmployeeId;
                user.Status = model.Status.ToString();
                user.Role = "user";
                string uniqueFileName = null;
                if (model.Avatar != null)
                {
                    // Get the uploaded file name
                    string fileName = Path.GetFileName(model.Avatar.FileName);

                    Random random = new();
                    string randomString = new(
                        Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                  .Select(s => s[random.Next(s.Length)])
                                  .ToArray()
                    );

                    // Create a unique file name to avoid overwriting existing files
                    uniqueFileName = randomString + "_" + fileName;

                    // Set the file path where the uploaded file will be saved
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                    // Save the uploaded file to the specified directory
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Avatar.CopyTo(fileStream);
                    }
                    user.Avatar = "/images/" + uniqueFileName;
                }
                _db.Users.Add(user);
                _db.SaveChanges();
                return true;
            }
            else
            {
                if (model.UserId != null || model.UserId != 0)
                {
                    User? editUser = _db.Users.FirstOrDefault(c => c.UserId == model.UserId);
                    editUser.FirstName = model.FirstName;
                    editUser.LastName = model.LastName;
                    editUser.Email = model.Email;
                    editUser.ProfileText = model.ProfileText;
                    editUser.Password = model.Password;
                    editUser.Department = model.Department;
                    editUser.CityId = model.CityId;
                    editUser.CountryId = model.CountryId;
                    editUser.CreatedAt = DateTime.Now;
                    editUser.EmployeeId = model.EmployeeId;
                    editUser.Status = model.Status.ToString();
                    string uniqueFileName = null;
                    if (model.Avatar != null)
                    {
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(model.Avatar.FileName);

                        Random random = new();
                        string randomString = new(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Avatar.CopyTo(fileStream);
                        }
                        editUser.Avatar = "/images/" + uniqueFileName;
                    }
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool UserExists(string email)
        {
            User UserExists = _db.Users.FirstOrDefault(User => User.Email == email);
            if(UserExists == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Skill AddSkill(string SkillName,int Status)
        {
            Skill NewSkill = new()
            {
                SkillName = SkillName,
                Status = (byte)Status
            };
            _db.Skills.Add(NewSkill);
            Save();
            return NewSkill;
        }
        public Skill EditSkill(int skillid, int Status, string SkillName)
        {
            Skill editskill = _db.Skills.FirstOrDefault(t => t.SkillId == skillid);
            if (editskill is not null)
            {
                editskill.Status = (byte)Status;
                editskill.SkillName = SkillName;
                Save();
                return editskill;
            }
            else
            {
                return null;
            }
        }

        public bool DeleteSkill(int skill_id)
        {
            Skill deleteskilll = _db.Skills.FirstOrDefault(t => t.SkillId == skill_id);
            if (deleteskilll is not null)
            {
                deleteskilll.DeletedAt = DateTime.Now;
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public CrudViewModel GetAllStories()
        {
            var stories = _db.Stories.Where(s => s.Status != "DRAFT" && s.DeletedAt == null)
                            .Select(s => new Story
                            {
                                StoryId = s.StoryId,
                                Title = s.Title,
                                Status = s.Status,
                                // Include related Mission entity
                                Mission = _db.Missions.FirstOrDefault(m => m.MissionId == s.MissionId),
                                // Include related User entity
                                User = _db.Users.FirstOrDefault(u => u.UserId == s.UserId)
                            })
                            .ToList();

            return new CrudViewModel
            {
                Stories = stories
            };
        }
        public StoryViewModel GetStoryDetail(int StoryId)
        {
            var storyQuery = _db.Stories.Where(c => c.StoryId == StoryId);
            var storyMediaQuery = _db.StoryMedia.Where(sm => sm.StoryId == StoryId);
            var userQuery = _db.Users;

            var story = storyQuery.FirstOrDefault();
            var storyMedia = storyMediaQuery.ToList();
            var users = userQuery.ToList();

            return new StoryViewModel { Stories = story, All_volunteers = users };
        }
        public bool DeleteStory(int StoryId)
        {
            Story deletestory = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if(deletestory is not null)
            {
                deletestory.DeletedAt = DateTime.Now;
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public CrudViewModel GetAllMissionApplications()
        {
            var missionapplications = _db.MissionApplications.Select(ma => new MissionApplication
            {
                MissionApplicationId = ma.MissionApplicationId,
                AppliedAt = ma.AppliedAt,
                ApprovalStatus = ma.ApprovalStatus,
                Mission = _db.Missions.FirstOrDefault(m => m.MissionId == ma.MissionId),
                User = _db.Users.FirstOrDefault(m => m.UserId == ma.UserId)
            }).ToList();
            return new CrudViewModel
            {
                MissionApplications = missionapplications
            };
        }

        public bool ApproveMissionApplication(int MissionApplicationId, string MissionLink)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == missionapplication.MissionId).Select(mission => mission.Title).ToList();
            if (missionapplication != null)
            {
                missionapplication.ApprovalStatus = "APPROVE";
                _db.Notifications.Add(
                    new Notification
                    {
                        UserId = missionapplication.UserId,
                        MissionId = missionapplication.MissionId,
                        Message = "Your mission application for " + @missiontitle.FirstOrDefault() + " has been approved!!",
                        UserAvatar = "/images/right.png",
                        Status = "NOT SEEN",
                        MissionStatus = "APPROVE",
                        NotificationSettingId = 9
                    });
                Save();
                var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == missionapplication.UserId
                                                               && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                if (emailnotification != null && emailnotification != 0)
                {
                    var email = _db.Users.Where(User => User.UserId == missionapplication.UserId).Select(User => User.Email).FirstOrDefault();
                    var subject = "Mission Application Approved";
                    
                    var body = "Follow this link to see the mission " + MissionLink;
                    bool sentemailnotification = repository.SendEmail(email, subject, body);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeclineMissionApplication(int MissionApplicationId,string MissionLink)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == missionapplication.MissionId).Select(mission => mission.Title);
            if (missionapplication != null)
            {
                missionapplication.ApprovalStatus = "DECLINE";
                _db.Notifications.Add(
                    new Notification
                    {
                        UserId = missionapplication.UserId,
                        MissionId = missionapplication.MissionId,
                        Message = "Your mission application for " + @missiontitle.FirstOrDefault() + " has been declined!!",
                        UserAvatar = "/images/cancel1.png",
                        Status = "NOT SEEN",
                        MissionStatus = "DECLINE",
                        NotificationSettingId = 9
                    });
                Save();
                var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == missionapplication.UserId
                                                               && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                if (emailnotification != null && emailnotification != 0)
                {
                    var email = _db.Users.Where(User => User.UserId == missionapplication.UserId).Select(User => User.Email).FirstOrDefault();
                    var subject = "Mission Application Declined";

                    var body = "Follow this link to see the mission " + MissionLink;
                    bool sentemailnotification = repository.SendEmail(email, subject, body);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PublishStory(int StoryId,string StoryLink)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if(story != null)
            {
                story.Status = "PUBLISHED";
                _db.Notifications.Add(
                    new Notification
                    {
                      UserId = story.UserId,
                      StoryId = story.StoryId,
                      Message = "Your story with title " +@story.Title + " has been approved!!",
                      UserAvatar = "/images/right.png",
                      Status = "NOT SEEN",
                      StoryStatus = "PUBLISHED",
                      NotificationSettingId = 5
                    });
                Save();
                var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == story.UserId
                                               && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                if (emailnotification != null && emailnotification != 0)
                {
                    var email = _db.Users.Where(User => User.UserId == story.UserId).Select(User => User.Email).FirstOrDefault();
                    var subject = "Your story has been approved!!";

                    var body = "Follow this link to see the story " + StoryLink;
                    bool sentemailnotification = repository.SendEmail(email, subject, body);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeclineStory(int StoryId, string StoryLink)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if (story != null)
            {
                story.Status = "DECLINED";
                _db.Notifications.Add(
                   new Notification
                   {
                       UserId = story.UserId,
                       StoryId = story.StoryId,
                       Message = "Your story with title " + @story.Title + " has been declined!!",
                       UserAvatar = "/images/cross.png",
                       Status = "NOT SEEN",
                       StoryStatus = "DECLINED",
                       NotificationSettingId = 5
                   });
                Save();
                var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == story.UserId
                                              && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                if (emailnotification != null && emailnotification != 0)
                {
                    var email = _db.Users.Where(User => User.UserId == story.UserId).Select(User => User.Email).FirstOrDefault();
                    var subject = "Your story has been declined it doesn't meet our policies!!";

                    var body = "Follow this link to see our privacy policy " + StoryLink;
                    bool sentemailnotification = repository.SendEmail(email, subject, body);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public CrudViewModel GetAllCmsPages()
        {
            List<CmsPage> cmspages = _db.CmsPages.Where(Cmspage => Cmspage.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                CmsPages = cmspages
            };
        }
        public bool AddCmsPage(AddCMSViewModel addCMSViewModel)
        {
            if(addCMSViewModel.CMSPageId == 0)
            {
                CmsPage newcmsPage = new()
                {
                    Title = addCMSViewModel.Title,
                    Description = addCMSViewModel.Description,
                    Slug = addCMSViewModel.Slug,
                    Status = addCMSViewModel.Status
                };
                _db.CmsPages.Add(newcmsPage);
                Save();
                return true;
            }
            else
            {
                CmsPage editCmsPage = _db.CmsPages.FirstOrDefault(CmsPage => CmsPage.CmsPageId == addCMSViewModel.CMSPageId);
                editCmsPage.Title = addCMSViewModel.Title;
                editCmsPage.Description = addCMSViewModel.Description;
                editCmsPage.Slug = addCMSViewModel.Slug;
                editCmsPage.Status = addCMSViewModel.Status;
                editCmsPage.UpdatedAt = DateTime.Now;
                Save();
                return true;

            }
            
        }

        public CmsPage GetCmsPageById(long CMSPageId)
        {
            CmsPage cmsPage = _db.CmsPages.FirstOrDefault(CmsPage => CmsPage.CmsPageId == CMSPageId);
            return cmsPage;
        }

        public bool DeleteCMSPage(long CMSPageId)
        {
            CmsPage deleteCMSPage = _db.CmsPages.FirstOrDefault(CmsPage => CmsPage.CmsPageId == CMSPageId);
            if (deleteCMSPage is not null)
            {
                deleteCMSPage.DeletedAt = DateTime.Now;
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public long AddMission(AddMissionViewModel addMissionViewModel)
        {
            if (addMissionViewModel.MissionId == 0) //Add Mission
            {
                if (addMissionViewModel.MissionType == "Time" || addMissionViewModel.MissionType == "TIME")
                {
                    Mission NewMission = new()
                    {
                        MissionType = "TIME",
                        Title = addMissionViewModel.Title,
                        ShortDescription = addMissionViewModel.ShortDescription,
                        Description = addMissionViewModel.Description,
                        Deadline = addMissionViewModel.Deadline,
                        StartDate = addMissionViewModel.StartDate,
                        EndDate = addMissionViewModel.EndDate,
                        CityId = addMissionViewModel.CityId,
                        CountryId = addMissionViewModel.CountryId,
                        SeatsLeft = addMissionViewModel.SeatsLeft,
                        ThemeId = addMissionViewModel.ThemeId,
                        Availability = addMissionViewModel.Availability,
                        OrganizationName = addMissionViewModel.OrganizationName,
                        OrganizationDetail = addMissionViewModel.OrganizationDetail
                    };
                    _db.Missions.Add(NewMission);
                    Save();
                    long NewMissionId = NewMission.MissionId;
                    foreach (var item in addMissionViewModel.Media)
                    {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);

                        Random random = new();
                        string randomString = new(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );
                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                        }
                        _db.MissionMedia.Add(new MissionMedium
                        {
                            MissionId = NewMissionId,
                            MediaType = "img",
                            MediaPath = "/images/" + uniqueFileName, // Save the unique file name in the database
                            MediaName = fileName,
                            // set the first instance of item in Media as default and for remaining set default as 0
                        });

                    }
                    _db.MissionMedia.Add(new MissionMedium
                    {
                        MissionId = NewMissionId,
                        MediaType = "vid",
                        MediaPath = addMissionViewModel.YoutubeUrl,
                        MediaName = "youtubevideo"
                    });
                    if (addMissionViewModel.Selected_Skills != null && addMissionViewModel.Selected_Skills != "")
                    {
                        List<MissionSkill> mission_skills = _db.MissionSkills.Where(c => c.MissionId == NewMissionId).ToList();
                        if (mission_skills.Count > 0)
                        {
                            _db.RemoveRange(mission_skills);
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = NewMissionId });
                            }
                        }
                        else
                        {
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = NewMissionId });
                            }
                        }
                    }
                    if (addMissionViewModel.MissionDocuments.Count > 0)
                    {
                        foreach (var item in addMissionViewModel.MissionDocuments)
                        {
                            string uniqueFileName = null;
                            // Get the uploaded file name
                            string fileName = Path.GetFileName(item.FileName);
                            string filetype = Path.GetExtension(item.FileName);

                            Random random = new();
                            string randomString = new(
                                Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray()
                            );
                            // Create a unique file name to avoid overwriting existing files
                            uniqueFileName = randomString + "_" + fileName;

                            // Set the file path where the uploaded file will be saved
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", uniqueFileName);

                            // Save the uploaded file to the specified directory
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(fileStream);
                            }
                            _db.MissionDocuments.Add(new MissionDocument
                            {
                                MissionId = NewMissionId,
                                DocumentType = filetype,
                                DocumentPath = "/documents/" + uniqueFileName, // Save the unique file name in the database
                                DocumentName = fileName,
                                // set the first instance of item in Media as default and for remaining set default as 0
                            });
                        }
                    }
                    //Notifying Users
                    var NotifyingUserIds = _db.Users.Where(User => User.DeletedAt == null).Select(User => User.UserId).ToList();
                    foreach(var userid in NotifyingUserIds)
                    {
                       _db.Notifications.Add(
                                    new Notification
                                    {
                                        UserId = userid,
                                        MissionId = NewMission.MissionId,
                                        Message = "New Mission - " + NewMission.Title,
                                        UserAvatar = "/images/add.png",
                                        Status = "NOT SEEN",
                                        NotificationSettingId = 7
                                    });
                        
                    }
      
                    Save();
                    return NewMissionId;
                }
            
            else if (addMissionViewModel.MissionType == "Go" || addMissionViewModel.MissionType == "GO")
            {
                Mission NewMission = new()
                {
                    MissionType = "GO",
                    Title = addMissionViewModel.Title,
                    ShortDescription = addMissionViewModel.ShortDescription,
                    Description = addMissionViewModel.Description,
                    Deadline = addMissionViewModel.Deadline,
                    CityId = addMissionViewModel.CityId,
                    CountryId = addMissionViewModel.CountryId,
                    ThemeId = addMissionViewModel.ThemeId,
                    Availability = addMissionViewModel.Availability,
                    OrganizationName = addMissionViewModel.OrganizationName,
                    OrganizationDetail = addMissionViewModel.OrganizationDetail,
                    GoalMotto = addMissionViewModel.Goal_Motto,
                    StartDate = addMissionViewModel.StartDate,
                    EndDate = addMissionViewModel.EndDate,
                    GoalAcheived = addMissionViewModel.Goal_Achieved
                };
                _db.Missions.Add(NewMission);
                Save();
                long NewMissionId = NewMission.MissionId;
                foreach (var item in addMissionViewModel.Media)
                {
                    string uniqueFileName = null;
                    // Get the uploaded file name
                    string fileName = Path.GetFileName(item.FileName);

                    Random random = new();
                    string randomString = new(
                        Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                  .Select(s => s[random.Next(s.Length)])
                                  .ToArray()
                    );
                    // Create a unique file name to avoid overwriting existing files
                    uniqueFileName = randomString + "_" + fileName;

                    // Set the file path where the uploaded file will be saved
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                    // Save the uploaded file to the specified directory
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(fileStream);
                    }
                    _db.MissionMedia.Add(new MissionMedium
                    {
                        MissionId = NewMissionId,
                        MediaType = "img",
                        MediaPath = "/images/" + uniqueFileName, // Save the unique file name in the database
                        MediaName = fileName,
                        // set the first instance of item in Media as default and for remaining set default as 0
                    });

                }
                _db.MissionMedia.Add(new MissionMedium
                {
                    MissionId = NewMissionId,
                    MediaType = "vid",
                    MediaPath = addMissionViewModel.YoutubeUrl,
                    MediaName = "youtubevideo"
                });
                    if (addMissionViewModel.Selected_Skills != null && addMissionViewModel.Selected_Skills != "")
                    {
                        List<MissionSkill> mission_skills = _db.MissionSkills.Where(c => c.MissionId == NewMissionId).ToList();
                        if (mission_skills.Count > 0)
                        {
                            _db.RemoveRange(mission_skills);
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = NewMissionId });
                            }
                        }
                        else
                        {
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = NewMissionId });
                            }
                        }
                    }
                    if (addMissionViewModel.MissionDocuments.Count > 0)
                    {
                        foreach (var item in addMissionViewModel.MissionDocuments)
                        {
                            string uniqueFileName = null;
                            // Get the uploaded file name
                            string fileName = Path.GetFileName(item.FileName);
                            string filetype = Path.GetExtension(item.FileName);

                            Random random = new();
                            string randomString = new(
                                Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray()
                            );
                            // Create a unique file name to avoid overwriting existing files
                            uniqueFileName = randomString + "_" + fileName;

                            // Set the file path where the uploaded file will be saved
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", uniqueFileName);

                            // Save the uploaded file to the specified directory
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(fileStream);
                            }
                            _db.MissionDocuments.Add(new MissionDocument
                            {
                                MissionId = NewMissionId,
                                DocumentType = filetype,
                                DocumentPath = "/documents/" + uniqueFileName, // Save the unique file name in the database
                                DocumentName = fileName,
                                // set the first instance of item in Media as default and for remaining set default as 0
                            });
                        }
                    }
                    //Notifying Users
                    var NotifyingUserIds = _db.Users.Where(User => User.DeletedAt == null).Select(User => User.UserId).ToList();
                    foreach (var userid in NotifyingUserIds)
                    {
                        _db.Notifications.Add(
                                     new Notification
                                     {
                                         UserId = userid,
                                         MissionId = NewMission.MissionId,
                                         Message = "New Mission - " + NewMission.Title,
                                         UserAvatar = "/images/add.png",
                                         Status = "NOT SEEN",
                                         NotificationSettingId = 7
                                     });
                    }
                    Save();
                    return NewMissionId;
                }

                else
                {
                    return 0;
                }
            }
            else
            {
                if (addMissionViewModel.MissionType == "Time" || addMissionViewModel.MissionType == "TIME")
                {
                    Mission editTimeMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == addMissionViewModel.MissionId);
                    editTimeMission.MissionType = "TIME";
                    editTimeMission.Title = addMissionViewModel.Title;
                    editTimeMission.ShortDescription = addMissionViewModel.ShortDescription;
                    editTimeMission.Description = addMissionViewModel.Description;
                    editTimeMission.Deadline = addMissionViewModel.Deadline;
                    editTimeMission.StartDate = addMissionViewModel.StartDate;
                    editTimeMission.EndDate = addMissionViewModel.EndDate;
                    editTimeMission.CityId = addMissionViewModel.CityId;
                    editTimeMission.CountryId = addMissionViewModel.CountryId;
                    editTimeMission.SeatsLeft = addMissionViewModel.SeatsLeft;
                    editTimeMission.ThemeId = addMissionViewModel.ThemeId;
                    editTimeMission.Availability = addMissionViewModel.Availability;
                    editTimeMission.OrganizationName = addMissionViewModel.OrganizationName;
                    editTimeMission.OrganizationDetail = addMissionViewModel.OrganizationDetail;
                    Save();
                    long EditMissionId = addMissionViewModel.MissionId;
                    //Deleting existing media 
                    List<MissionMedium> mission_media = _db.MissionMedia.Where(c => c.MissionId == EditMissionId).ToList();
                    foreach (var medium in mission_media)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", medium.MediaPath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        _db.MissionMedia.Remove(medium);
                    }
                    List<MissionDocument> missionDocuments = _db.MissionDocuments.Where(c => c.MissionId == EditMissionId).ToList();
                    foreach (var medium in missionDocuments)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", medium.DocumentPath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        _db.MissionDocuments.Remove(medium);
                    }

                    foreach (var item in addMissionViewModel.Media)
                    {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);
                        Random random = new();
                        string randomString = new(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );
                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                        }
                        _db.MissionMedia.Add(new MissionMedium
                        {
                            MissionId = EditMissionId,
                            MediaType = "img",
                            MediaPath = "/images/" + uniqueFileName, // Save the unique file name in the database
                            MediaName = fileName,
                            // set the first instance of item in Media as default and for remaining set default as 0
                        });
                    }
                    _db.MissionMedia.Add(new MissionMedium
                    {
                        MissionId = EditMissionId,
                        MediaType = "vid",
                        MediaPath = addMissionViewModel.YoutubeUrl,
                        MediaName = "youtubevideo"
                    });
                    if (addMissionViewModel.Selected_Skills != null && addMissionViewModel.Selected_Skills != "")
                    {
                        List<MissionSkill> mission_skills = _db.MissionSkills.Where(c => c.MissionId == EditMissionId).ToList();
                        if (mission_skills.Count > 0)
                        {
                            _db.RemoveRange(mission_skills);
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = EditMissionId });
                            }
                        }
                        else
                        {
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = EditMissionId });
                            }
                        }
                    }
                    if (addMissionViewModel.MissionDocuments.Count > 0)
                    {
                        foreach (var item in addMissionViewModel.MissionDocuments)
                        {
                            string uniqueFileName = null;
                            // Get the uploaded file name
                            string fileName = Path.GetFileName(item.FileName);
                            string filetype = Path.GetExtension(item.FileName);

                            Random random = new();
                            string randomString = new(
                                Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray()
                            );
                            // Create a unique file name to avoid overwriting existing files
                            uniqueFileName = randomString + "_" + fileName;

                            // Set the file path where the uploaded file will be saved
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", uniqueFileName);

                            // Save the uploaded file to the specified directory
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(fileStream);
                            }
                            _db.MissionDocuments.Add(new MissionDocument
                            {
                                MissionId = EditMissionId,
                                DocumentType = filetype,
                                DocumentPath = "/documents/" + uniqueFileName, // Save the unique file name in the database
                                DocumentName = fileName,
                                // set the first instance of item in Media as default and for remaining set default as 0
                            });
                        }
                    }
                    Save();
                    return 0;
                }
                else if (addMissionViewModel.MissionType == "Go" || addMissionViewModel.MissionType == "GO")
                {
                    Mission editGoalMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == addMissionViewModel.MissionId);
                    editGoalMission.MissionType = "GO";
                    editGoalMission.Title = addMissionViewModel.Title;
                    editGoalMission.ShortDescription = addMissionViewModel.ShortDescription;
                    editGoalMission.Description = addMissionViewModel.Description;
                    editGoalMission.Deadline = addMissionViewModel.Deadline;
                    editGoalMission.StartDate = addMissionViewModel.StartDate;
                    editGoalMission.EndDate = addMissionViewModel.EndDate;
                    editGoalMission.CityId = addMissionViewModel.CityId;
                    editGoalMission.CountryId = addMissionViewModel.CountryId;
                    editGoalMission.ThemeId = addMissionViewModel.ThemeId;
                    editGoalMission.Availability = addMissionViewModel.Availability;
                    editGoalMission.OrganizationName = addMissionViewModel.OrganizationName;
                    editGoalMission.OrganizationDetail = addMissionViewModel.OrganizationDetail;
                    editGoalMission.GoalMotto = addMissionViewModel.Goal_Motto;
                    editGoalMission.GoalAcheived = addMissionViewModel.Goal_Achieved;
                    Save();
                    long EditMissionId = editGoalMission.MissionId;
                    //Deleting existing media 
                    List<MissionMedium> mission_media = _db.MissionMedia.Where(c => c.MissionId == EditMissionId).ToList();
                    foreach (var medium in mission_media)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", medium.MediaPath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        _db.MissionMedia.Remove(medium);
                    }
                    List<MissionDocument> missionDocuments = _db.MissionDocuments.Where(c => c.MissionId == EditMissionId).ToList();
                    foreach (var medium in missionDocuments)
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", medium.DocumentPath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        _db.MissionDocuments.Remove(medium);
                    }
                    foreach (var item in addMissionViewModel.Media)
                    {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);

                        Random random = new();
                        string randomString = new(
                            Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                      .Select(s => s[random.Next(s.Length)])
                                      .ToArray()
                        );
                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = randomString + "_" + fileName;

                        // Set the file path where the uploaded file will be saved
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                        // Save the uploaded file to the specified directory
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(fileStream);
                        }
                        _db.MissionMedia.Add(new MissionMedium
                        {
                            MissionId = EditMissionId,
                            MediaType = "img",
                            MediaPath = "/images/" + uniqueFileName, // Save the unique file name in the database
                            MediaName = fileName,
                            // set the first instance of item in Media as default and for remaining set default as 0
                        });
                    }
                    _db.MissionMedia.Add(new MissionMedium
                    {
                        MissionId = EditMissionId,
                        MediaType = "vid",
                        MediaPath = addMissionViewModel.YoutubeUrl,
                        MediaName = "youtubevideo"
                    });
                    if (addMissionViewModel.Selected_Skills != null && addMissionViewModel.Selected_Skills != "")
                    {
                        List<MissionSkill> mission_skills = _db.MissionSkills.Where(c => c.MissionId == EditMissionId).ToList();
                        if (mission_skills.Count > 0)
                        {
                            _db.RemoveRange(mission_skills);
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = EditMissionId });
                            }
                        }
                        else
                        {
                            string[] skills = addMissionViewModel.Selected_Skills.Split(',');
                            foreach (var skill in skills)
                            {
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = EditMissionId });
                            }
                        }
                    }
                    if (addMissionViewModel.MissionDocuments.Count > 0)
                    {
                        foreach (var item in addMissionViewModel.MissionDocuments)
                        {
                            string uniqueFileName = null;
                            // Get the uploaded file name
                            string fileName = Path.GetFileName(item.FileName);
                            string filetype = Path.GetExtension(item.FileName);

                            Random random = new();
                            string randomString = new(
                                Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray()
                            );
                            // Create a unique file name to avoid overwriting existing files
                            uniqueFileName = randomString + "_" + fileName;

                            // Set the file path where the uploaded file will be saved
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents", uniqueFileName);

                            // Save the uploaded file to the specified directory
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(fileStream);
                            }
                            _db.MissionDocuments.Add(new MissionDocument
                            {
                                MissionId = EditMissionId,
                                DocumentType = filetype,
                                DocumentPath = "/documents/" + uniqueFileName, // Save the unique file name in the database
                                DocumentName = fileName,
                                // set the first instance of item in Media as default and for remaining set default as 0
                            });
                        }
                    }
                    Save();
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            
        }


        public AddMissionViewModel GetMissionById(long MissionId)
        {
            Mission mission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == MissionId);
            List<Skill> skills = _db.Skills.ToList();
            var missionskill = _db.MissionSkills.Where(ms => ms.MissionId == MissionId).ToList();
            var selectedSkills = string.Join(",", missionskill.Select(x => x.SkillId.ToString()));
            var Selectedskillnames = string.Join(",", missionskill.Select(y => y.Skill.SkillName.ToString()));
            MissionMedium Videourls = _db.MissionMedia.FirstOrDefault(missionmedium => missionmedium.MissionId == MissionId && missionmedium.MediaType == "vid");
            return new AddMissionViewModel
            {
                MissionType = mission.MissionType,
                Title = mission.Title,
                ShortDescription = mission.ShortDescription,
                Description = mission.Description,
                Deadline = mission.Deadline,
                StartDate = mission.StartDate ?? new DateTime(),
                EndDate = mission.EndDate ?? new DateTime(),
                CityId = mission.CityId,
                CountryId = mission.CountryId,
                SeatsLeft = mission.SeatsLeft ?? 0,
                ThemeId = mission.ThemeId,
                Availability = mission.Availability,
                OrganizationName = mission.OrganizationName,
                OrganizationDetail = mission.OrganizationDetail,
                Goal_Motto = mission.GoalMotto ?? string.Empty,
                Goal_Achieved = mission.GoalAcheived,
                Selected_Skills = selectedSkills,
                Selected_skill_names = Selectedskillnames,
                MissionSkills = missionskill,
                MissionId = mission.MissionId,
                MissionMedia = _db.MissionMedia.Where(missionmedium => missionmedium.MissionId == MissionId && missionmedium.MediaType == "img").ToList(),
                YoutubeUrl = Videourls?.MediaPath ?? String.Empty,
                ExistingDocuments = _db.MissionDocuments.Where(missiondocument => missiondocument.MissionId == MissionId).ToList()
            };
        }

        public bool DeleteMission(long MissionId)
        {
            Mission DeleteMission = _db.Missions.FirstOrDefault(Mission => Mission.MissionId == MissionId);
            //delete mission
            DeleteMission.DeletedAt = DateTime.Now;  // soft delete mission
            //missionapplication
            List<MissionApplication> DeleteMisssionApplications = _db.MissionApplications.Where(MissionApplication => MissionApplication.MissionId == MissionId).ToList();
            foreach(var MissionApplication in DeleteMisssionApplications)
            {
                MissionApplication.DeletedAt = DateTime.Now;
            }
            //relatedstories
            List<Story> DeleteMissionStory = _db.Stories.Where(Story => Story.MissionId == MissionId).ToList();
            foreach(var Story in DeleteMissionStory)
            {
                Story.DeletedAt = DateTime.Now;
            }
            //timesheets
            List<Timesheet> DeleteMissionTimesheets = _db.Timesheets.Where(Timesheet => Timesheet.MissionId == MissionId).ToList();
            foreach(var timesheet in DeleteMissionTimesheets)
            {
                timesheet.DeletedAt = DateTime.Now;
            }
            Save();
            return true;
        }

        public CrudViewModel GetAllBanners()
        {
            List<Banner> Banners = _db.Banners.Where(Banner => Banner.DeletedAt == null).ToList();
            return new CrudViewModel
            {
                Banners = Banners
            };
        }

        public bool Addbanner(AddBannerViewModel addBannerViewModel)
        {
            if(addBannerViewModel.BannerId == 0)
            {
                string uniqueFileName = null;
                // Get the uploaded file name
                string fileName = Path.GetFileName(addBannerViewModel.BannerImage.FileName);

                Random random = new();
                string randomString = new(
                    Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray()
                );
                // Create a unique file name to avoid overwriting existing files
                uniqueFileName = randomString + "_" + fileName;

                // Set the file path where the uploaded file will be saved
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                // Save the uploaded file to the specified directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    addBannerViewModel.BannerImage.CopyTo(fileStream);
                }
                Banner NewBanner = new()
                {
                    SortOrder = addBannerViewModel.SortOrder,
                    Text = addBannerViewModel.BannerText,
                    Image = "/images/" + uniqueFileName,
                };
                _db.Banners.Add(NewBanner);
                Save();
                return true;
            }
            else
            {
                Banner EditBanner = _db.Banners.FirstOrDefault(banner => banner.BannerId == addBannerViewModel.BannerId);
                EditBanner.SortOrder = addBannerViewModel.SortOrder;
                EditBanner.Text = addBannerViewModel.BannerText;
                string uniqueFileName = null;
                // Get the uploaded file name
                string fileName = Path.GetFileName(addBannerViewModel.BannerImage.FileName);

                Random random = new();
                string randomString = new(
                    Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 4)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray()
                );
                // Create a unique file name to avoid overwriting existing files
                uniqueFileName = randomString + "_" + fileName;

                // Set the file path where the uploaded file will be saved
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", uniqueFileName);

                // Save the uploaded file to the specified directory
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    addBannerViewModel.BannerImage.CopyTo(fileStream);
                }
                EditBanner.Image = "/images/" + uniqueFileName;
                EditBanner.UpdatedAt = DateTime.Now;
                Save();
                return true;
            }
            
        }

        public AddBannerViewModel GetBannerById(int BannerId)
        {
            Banner banner = _db.Banners.FirstOrDefault(banner => banner.BannerId == BannerId);
            return new AddBannerViewModel
            {
                BannerId = BannerId,
                SortOrder = banner.SortOrder,
                BannerText = banner.Text,
                BannerImagePath = banner.Image
            };
        }

        public bool DeleteBanner(int BannerId)
        {
            Banner deletebanner = _db.Banners.FirstOrDefault(banner => banner.BannerId == BannerId);
            deletebanner.DeletedAt = DateTime.Now;
            Save();
            return true;
        }

        public IEnumerable<AddBannerViewModel> GetBanners()
        {
            List<Banner> banners = _db.Banners.ToList();
            List<AddBannerViewModel> displaybanners = (from b in banners
                                                    orderby b.SortOrder
                                                    where b.DeletedAt == null
                                                    select new AddBannerViewModel
                                                    {
                                                        BannerId = (int)b.BannerId,
                                                        BannerImagePath = b.Image,
                                                        BannerText = b.Text,
                                                        SortOrder = b.SortOrder
                                                    }).ToList();

            return displaybanners;
        }
        
        public bool NotifyuserEmail(long userid)
        {
            var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == userid
                                                  && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
            if(emailnotification != null && emailnotification !=0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CrudViewModel GetAllTimesheets()
        {
            List<Timesheet> timesheets = _db.Timesheets.Select(timesheet => new Timesheet()
            {
                MissionId = timesheet.MissionId,
                Action = timesheet.Action,
                Mission = timesheet.Mission,
                DateVolunteered = timesheet.DateVolunteered,
                CreatedAt = timesheet.CreatedAt,
                User = timesheet.User,
                Notes = timesheet.Notes,
                Status = timesheet.Status,
                Time = timesheet.Time,
                TimesheetId = timesheet.TimesheetId

            }).ToList();
            return new CrudViewModel
            {
                Timesheets = timesheets
            };
        }

        public bool ApproveTimesheet(int TimesheetId, string MissionLink)
        {
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(ma => ma.TimesheetId == TimesheetId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == timesheet.MissionId).Select(mission => mission.Title).ToList();
            if (timesheet != null)
            {
                timesheet.Status = "APPROVED";
                if (timesheet.Time != null)
                {
                    _db.Notifications.Add(
                        new Notification
                        {
                            UserId = timesheet.UserId,
                            MissionId = timesheet.MissionId,
                            Message = "Your Volunteering Hours for " + @missiontitle.FirstOrDefault() + " has been approved!!",
                            UserAvatar = "/images/right.png",
                            Status = "NOT SEEN",
                            MissionStatus = "APPROVE",
                            NotificationSettingId = 11
                        });
                    Save();
                    var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == timesheet.UserId
                                                                   && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                    if (emailnotification != null && emailnotification != 0)
                    {
                        var email = _db.Users.Where(User => User.UserId == timesheet.UserId).Select(User => User.Email).FirstOrDefault();
                        var subject = "Volunteering Hours Approved";

                        var body = "Follow this link to see the mission " + MissionLink;
                        bool sentemailnotification = repository.SendEmail(email, subject, body);
                    }
                }
                else
                {
                    _db.Notifications.Add(
                        new Notification
                        {
                            UserId = timesheet.UserId,
                            MissionId = timesheet.MissionId,
                            Message = "Your Volunteering Goal Action for " + @missiontitle.FirstOrDefault() + " has been approved!!",
                            UserAvatar = "/images/right.png",
                            Status = "NOT SEEN",
                            MissionStatus = "APPROVE",
                            NotificationSettingId = 12
                        });
                    Save();
                    var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == timesheet.UserId
                                                                   && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                    if (emailnotification != null && emailnotification != 0)
                    {
                        var email = _db.Users.Where(User => User.UserId == timesheet.UserId).Select(User => User.Email).FirstOrDefault();
                        var subject = "Volunteering Goals Approved";

                        var body = "Follow this link to see the mission " + MissionLink;
                        bool sentemailnotification = repository.SendEmail(email, subject, body);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeclineTimesheet(int TimesheetId, string MissionLink)
        {
            Timesheet timesheet = _db.Timesheets.FirstOrDefault(ma => ma.TimesheetId == TimesheetId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == timesheet.MissionId).Select(mission => mission.Title).ToList();
            if (timesheet != null)
            {
                timesheet.Status = "DECLINED";
                if (timesheet.Time != null)
                {
                    _db.Notifications.Add(
                        new Notification
                        {
                            UserId = timesheet.UserId,
                            MissionId = timesheet.MissionId,
                            Message = "Your Volunteering Hours for " + @missiontitle.FirstOrDefault() + " has been declined!!",
                            UserAvatar = "/images/cross.png",
                            Status = "NOT SEEN",
                            MissionStatus = "Decline",
                            NotificationSettingId = 11
                        });
                    Save();
                    var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == timesheet.UserId
                                                                   && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                    if (emailnotification != null && emailnotification != 0)
                    {
                        var email = _db.Users.Where(User => User.UserId == timesheet.UserId).Select(User => User.Email).FirstOrDefault();
                        var subject = "Volunteering Hours declined";

                        var body = "Follow this link to see the mission " + MissionLink;
                        bool sentemailnotification = repository.SendEmail(email, subject, body);
                    }
                }
                else
                {
                    _db.Notifications.Add(
                        new Notification
                        {
                            UserId = timesheet.UserId,
                            MissionId = timesheet.MissionId,
                            Message = "Your Volunteering Goal Action for " + @missiontitle.FirstOrDefault() + " has been declined!!",
                            UserAvatar = "/images/cross.png",
                            Status = "NOT SEEN",
                            MissionStatus = "DECLINE",
                            NotificationSettingId = 12
                        });
                    Save();
                    var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == timesheet.UserId
                                                                   && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                    if (emailnotification != null && emailnotification != 0)
                    {
                        var email = _db.Users.Where(User => User.UserId == timesheet.UserId).Select(User => User.Email).FirstOrDefault();
                        var subject = "Volunteering Goals declined";

                        var body = "Follow this link to see the mission " + MissionLink;
                        bool sentemailnotification = repository.SendEmail(email, subject, body);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public long Getmissionidbytimesheetid(int timesheetid)
        {
            return _db.Timesheets.Where(timesheet => timesheet.TimesheetId == timesheetid).Select(timesheet => timesheet.MissionId).FirstOrDefault();
        }

        public long GetMissionidbymissionapplicationid(int missionapplicationid)
        {
            return _db.MissionApplications.Where(missionapplication => missionapplication.MissionApplicationId == missionapplicationid).Select(missionapplication => missionapplication.MissionId).FirstOrDefault();
        }

        public CrudViewModel GetAllComments()
        {
            List<Comment> comments = _db.Comments.Select(comment => new Comment()
            {
                ApprovalStatus = comment.ApprovalStatus,
                CommentId = comment.CommentId,
                CommentText = comment.CommentText,
                CreatedAt = comment.CreatedAt,
                Mission = comment.Mission,
                UserId = comment.UserId,
                MissionId = comment.MissionId,
                User = comment.User
            }).ToList();
            return new CrudViewModel
            {
                Comments = comments
            };
        }


        public bool PublishComment(int CommentId, string MissionLink)
        {
            Comment comment = _db.Comments.FirstOrDefault(ma => ma.CommentId == CommentId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == comment.MissionId).Select(mission => mission.Title).ToList();
            if (comment != null)
            {
                comment.ApprovalStatus = "PUBLISHED";

                    _db.Notifications.Add(
                        new Notification
                        {
                            UserId = comment.UserId,
                            MissionId = comment.MissionId,
                            Message = "Your Comment " + "<" + comment.CommentText + ">" + "has been published!!",
                            UserAvatar = "/images/right.png",
                            Status = "NOT SEEN",
                            MissionStatus = "APPROVE",
                            NotificationSettingId = 13
                        });
                    Save();
                    var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == comment.UserId
                                                                   && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                    if (emailnotification != null && emailnotification != 0)
                    {
                        var email = _db.Users.Where(User => User.UserId == comment.UserId).Select(User => User.Email).FirstOrDefault();
                        var subject = "Comment Approved";

                        var body = "Follow this link to see the mission and comment " + MissionLink;
                        bool sentemailnotification = repository.SendEmail(email, subject, body);
                    }
                
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeclineComment(int CommentId, string PolicyLink)
        {
            Comment comment = _db.Comments.FirstOrDefault(ma => ma.CommentId == CommentId);
            var missiontitle = _db.Missions.Where(mission => mission.MissionId == comment.MissionId).Select(mission => mission.Title).ToList();
            if (comment != null)
            {
                comment.ApprovalStatus = "DECLINED";

                _db.Notifications.Add(
                    new Notification
                    {
                        UserId = comment.UserId,
                        MissionId = comment.MissionId,
                        Message = "Your Comment " + "<" + comment.CommentText + ">" + " has been declined!!",
                        UserAvatar = "/images/cancel1.png",
                        Status = "NOT SEEN",
                        MissionStatus = "decline",
                        NotificationSettingId = 13
                    });
                Save();
                var emailnotification = _db.UserNotificationSettings.Where(UserNotificationSetting => UserNotificationSetting.UserId == comment.UserId
                                                               && UserNotificationSetting.NotificationSettingId == 10).Select(UserNotificationSetting => UserNotificationSetting.UserNotificationSettingId).FirstOrDefault();
                if (emailnotification != null && emailnotification != 0)
                {
                    var email = _db.Users.Where(User => User.UserId == comment.UserId).Select(User => User.Email).FirstOrDefault();
                    var subject = "Your comment " + "<" + comment.CommentText + ">" + " Declined as it doesn't meet our policies";

                    var body = "Follow this link to see our privacy policy " + PolicyLink + " Make sure to understand it correctly as doing inappropriate comments can get you banned";
                    bool sentemailnotification = repository.SendEmail(email, subject, body);
                }


                return true;
            }
            else
            {
                return false;
            }
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
