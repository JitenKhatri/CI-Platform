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
            var missions = _db.Missions.ToList();
            return new CrudViewModel
            {
                Missions = missions
            };
        }

        public CrudViewModel GetAllThemes()
        {
            var missionThemes = _db.MissionThemes.ToList();
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
                _db.MissionThemes.Remove(deletetheme);
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
            List<Skill> Skills = _db.Skills.ToList();
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
                return false;
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
                _db.Skills.Remove(deleteskilll);
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
            CmsPage newcmsPage = new CmsPage
            {
                Title = addCMSViewModel.Title,
                Description = addCMSViewModel.Description,
                Slug = addCMSViewModel.Slug,
                Status = addCMSViewModel.Status.ToString()
            };
            _db.CmsPages.Add(newcmsPage);
            Save();
            return true;
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
