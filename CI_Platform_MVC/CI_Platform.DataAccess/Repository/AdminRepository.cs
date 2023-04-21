using CI_Platform.Areas.Admin.ViewModels;
using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly CiPlatformContext _db;
        public AdminRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
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
            MissionTheme NewTheme = new MissionTheme
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
            User? User = _db.Users.FirstOrDefault(c => c.Email == model.Email);
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
                string uniqueFileName = null;
                if (model.Avatar != null)
                {
                    // Get the uploaded file name
                    string fileName = Path.GetFileName(model.Avatar.FileName);

                    // Create a unique file name to avoid overwriting existing files
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

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

                        // Create a unique file name to avoid overwriting existing files
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

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
            Skill NewSkill = new Skill
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

        public bool ApproveMissionApplication(int MissionApplicationId)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            if(missionapplication != null)
            {
                missionapplication.ApprovalStatus = "APPROVE";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeclineMissionApplication(int MissionApplicationId)
        {
            MissionApplication missionapplication = _db.MissionApplications.FirstOrDefault(ma => ma.MissionApplicationId == MissionApplicationId);
            if (missionapplication != null)
            {
                missionapplication.ApprovalStatus = "DECLINE";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool PublishStory(int StoryId)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if(story != null)
            {
                story.Status = "PUBLISHED";
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeclineStory(int StoryId)
        {
            Story story = _db.Stories.FirstOrDefault(s => s.StoryId == StoryId);
            if (story != null)
            {
                story.Status = "DECLINED";
                Save();
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
                CmsPage newcmsPage = new CmsPage
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

        public bool AddMission(AddMissionViewModel addMissionViewModel)
        {
            if (addMissionViewModel.MissionId == 0)
            {
                if (addMissionViewModel.MissionType == "Time")
                {
                    Mission NewMission = new Mission
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

                        Random random = new Random();
                        string randomString = new string(
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
                    Save();
                    return true;
                }
                else if (addMissionViewModel.MissionType == "Goal")
                {
                    Mission NewMission = new Mission
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
                    };
                    _db.Missions.Add(NewMission);
                    Save();
                    long NewMissionId = NewMission.MissionId;
                    foreach (var item in addMissionViewModel.Media)
                    {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);

                        Random random = new Random();
                        string randomString = new string(
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
                if (addMissionViewModel.MissionType == "Time")
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
                    if (mission_media.Count > 0)
                    {
                        _db.RemoveRange(mission_media);
                    }
                        foreach (var item in addMissionViewModel.Media)
                       {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);
                        Random random = new Random();
                        string randomString = new string(
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
                    Save();
                    return true;
                }
                else if (addMissionViewModel.MissionType == "Goal")
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
                    Save();
                    long EditMissionId = editGoalMission.MissionId;
                    //Deleting existing media 
                    List<MissionMedium> mission_media = _db.MissionMedia.Where(c => c.MissionId == EditMissionId).ToList();
                    if (mission_media.Count > 0)
                    {
                        _db.RemoveRange(mission_media);
                    }
                    foreach (var item in addMissionViewModel.Media)
                    {
                        string uniqueFileName = null;
                        // Get the uploaded file name
                        string fileName = Path.GetFileName(item.FileName);

                        Random random = new Random();
                        string randomString = new string(
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
                                _db.MissionSkills.Add(new MissionSkill { SkillId = int.Parse(skill), MissionId = EditMissionId});
                            }
                        }
                    }
                    Save();
                    return true;
                }
                else
                {
                    return false;
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
                Selected_Skills = selectedSkills,
                Selected_skill_names = Selectedskillnames,
                MissionSkills = missionskill,
                MissionId = mission.MissionId,
                MissionMedia = _db.MissionMedia.Where(missionmedium => missionmedium.MissionId == MissionId).ToList(),
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

                Random random = new Random();
                string randomString = new string(
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
                Banner NewBanner = new Banner
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

                Random random = new Random();
                string randomString = new string(
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
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
