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
    public class StoryRepository : Repository<Story>, IStoryRepository
    {
        private readonly CiPlatformContext _db;
        public StoryRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }
        public List<StoryViewModel> GetAllStories(int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Skip(skipCount)
                                          .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<User> users = _db.Users.ToList();
            List<MissionTheme> themes = _db.MissionThemes.ToList();
            List<Mission> mission = _db.Missions.ToList();
            List<City> cities = _db.Cities.ToList();
            List<Country> countries = _db.Countries.ToList();
            var Stories = (from s in stories
                           join i in image on s.StoryId equals i.StoryId into data
                           from i in data.DefaultIfEmpty().Take(1)
                           select new StoryViewModel { image=i,Stories=s,Countries=countries,Cities=cities }).ToList();
            return Stories;
        }

        public List<StoryViewModel> GetFilteredStories(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, string searchtext=null ,int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Skip(skipCount)
                                          .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<City> city = _db.Cities.ToList();

            List<User> users = _db.Users.ToList();
            List<Story> story = _db.Stories.ToList();
            List<MissionSkill> missionskills = _db.MissionSkills.ToList();
            List<Skill> skills = _db.Skills.ToList();
            List<Mission> mission = _db.Missions.ToList();

            if(!String.IsNullOrEmpty(searchtext))
            {
                stories = (from s in stories
                           select s).Where(s => s.Title.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))
                           || s.Description.ToLower().Replace(" ", "").Contains(searchtext.ToLower().Replace(" ", ""))).ToList();
            }
            if (Countries.Count > 0)
            {
                city = (from c in cities
                        where Countries.Contains(c.Country.Name)
                        select c).ToList();
            }
            else
            {
                city = cities;
            }
            story = (from s in stories
                     where ((Cities.Count == 0 || Cities.Contains(s.Mission.City.Name)) &&
                            (Countries.Count == 0 || Countries.Contains(s.Mission.Country.Name)) &&
                            (Themes.Count == 0 || Themes.Contains(s.Mission.Theme.Title)) && (Skills.Count == 0 || Skills.All(sm => s.Mission.MissionSkills.Any(k => k.Skill.SkillName == sm))))
                     select s).ToList();
           
            var Stories = (from s in story
                            join i in image on s.StoryId equals i.StoryId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new StoryViewModel { image = i, Stories = s, Countries = countries, Cities = city, Story_city = s.Mission.City.Name, Story_theme = s.Mission.Theme.Title }).ToList();
            return Stories;

        }

        public List<City> CityCascade(long countryid)
        {
            var cities = _db.Cities
                .Where(c => c.CountryId == countryid)
                .ToList();

            return cities;
        }
    }
}
