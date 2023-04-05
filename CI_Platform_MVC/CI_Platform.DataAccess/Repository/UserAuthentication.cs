using CI_Platform.DataAccess.Repository.IRepository;
using CI_Platform.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.DataAccess.Repository
{
    public class UserAuthentication : Repository<User>, IUserAuthentication
    {
        private readonly CiPlatformContext _db;
        public UserAuthentication(CiPlatformContext db) : base(db)
        {
            _db = db;
        }
        void IUserAuthentication.Add(PasswordReset resetPasswordInfo)
        {
            _db.Add(resetPasswordInfo);
        }
        //void IUserAuthentication.Update(PasswordReset resetPasswordInfo)
        //{
        //    _db.Update(resetPasswordInfo);
        //}
        public User ResetPassword(string password, long id)
        {
            User user = _db.Users.Find(id);
            if (user == null)
            {
                return null;
            }
            else
            {
                user.Password = password;
                user.UpdatedAt = DateTime.Now;
                return user;
            }
        }

        public EditProfileViewModel GetUser(long UserId,int country)
        {
            if(country == 0)
            {
                User user = _db.Users.Find(UserId);
                List<City> cities = _db.Cities.ToList();
                List<Country> countries = _db.Countries.ToList();
                List<Skill> skills = _db.Skills.ToList();
                return new EditProfileViewModel
                {
                    User = user,
                    Countries = countries,
                    Cities= cities,
                    Skills = skills,
                    WhyIVolunteer = user.WhyIVolunteer,
                    ProfileText = user.ProfileText
                };
            }
            else
            {

                List<City> cities = _db.Cities.Where(c => c.CountryId == country).ToList();
                return new EditProfileViewModel { Cities = cities };
            }

            
        }

        public bool ChangePassword(long UserId , string password)
        {
            User user = _db.Users.Find(UserId);
            if(user == null)
            {
                return false;
            }
            else
            {
                user.Password = password;
                user.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
                return true;
            }

        }

        public bool UpdateProfile(EditProfileViewModel model,long user_id)
        {
            User? user = _db.Users.FirstOrDefault(c => c.UserId == user_id);
            if(user is not null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Title = model.Title;
                user.WhyIVolunteer = model.WhyIVolunteer;
                //user.Availablity = model.Availablity;
                user.ProfileText = model.ProfileText;
                user.LinkedInUrl = model.LinkedInUrl;
                user.Department = model.Department;
                user.CityId = model.CityId;
                user.CountryId = model.CountryId;
                user.UpdatedAt = DateTime.Now;
                if (model.Selected_Skills is not null || model.Selected_Skills!= "")
                {
                    List<UserSkill> user_skills = _db.UserSkills.Where(c => c.UserId == user_id).ToList();
                    if (user_skills.Count > 0)
                    {
                        _db.RemoveRange(user_skills);
                        string[] skills = model.Selected_Skills.Split(',');
                        foreach (var skill in skills)
                        {
                            _db.UserSkills.Add(new UserSkill { SkillId = int.Parse(skill), UserId = user_id });
                        }
                    }
                    else
                    {
                        string[] skills = model.Selected_Skills.Split(',');
                        foreach (var skill in skills)
                        {
                            _db.UserSkills.Add(new UserSkill { SkillId = int.Parse(skill), UserId = user_id });
                        }
                    }
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
        
}

