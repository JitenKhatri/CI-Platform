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

        public List<StoryViewModel> GetFilteredStories(List<string> Countries, List<string> Cities, List<string> Themes, List<string> Skills, int page = 1, int pageSize = 6)
        {
            int skipCount = (page - 1) * pageSize;
            List<Story> stories = _db.Stories.Skip(skipCount)
                                          .Take(pageSize).ToList();
            List<StoryMedium> image = _db.StoryMedia.ToList();
            List<MissionTheme> theme = _db.MissionThemes.ToList();
            List<Country> countries = _db.Countries.ToList();
            List<City> cities = _db.Cities.ToList();
            List<City> city = new List<City>();
            List<Story> story = new List<Story>();
            List<MissionSkill> missionskills = _db.MissionSkills.ToList();
            List<Skill> skills = _db.Skills.ToList();
            List<Mission> mission = new List<Mission>();
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
            if (Cities.Count > 0)
            {
                story = (from s in stories
                           where Cities.Contains(s.Mission.City.Name) || Themes.Contains(s.Mission.Theme.Title)
                           select s).ToList();
                //var skill_stories = (from s in missionskills
                //                      where Skills.Contains(s.Skill.SkillName)
                //                      select s.Mission).ToList();
                //foreach (var skill_mission in skill_missions)
                //{
                //    if (!mission.Contains(skill_mission))
                //    {
                //        mission.Add(skill_mission);
                //    }
                //}
            }
            else if (Countries.Count > 0 || Themes.Count > 0 || Skills.Count > 0)
            {
                story = (from s in stories
                           where Countries.Contains(s.Mission.Country.Name) || Cities.Contains(s.Mission.City.Name) || Themes.Contains(s.Mission.Theme.Title)
                           select s).ToList();
                //var skill_missions = (from s in missionskills
                //                      where Skills.Contains(s.Skill.SkillName)
                //                      select s.Mission).ToList();
                //foreach (var skill_mission in skill_missions)
                //{
                //    if (!mission.Contains(skill_mission))
                //    {
                //        mission.Add(skill_mission);
                //    }
                //}
            }
            else
            {
                story = stories;
            }
            var Stories = (from s in story
                            join i in image on s.StoryId equals i.StoryId into data
                            from i in data.DefaultIfEmpty().Take(1)
                            select new StoryViewModel { image = i, Stories = s, Countries = countries, Cities = city, Story_city = s.Mission.City.Name, Story_theme = s.Mission.Theme.Title }).ToList();
            return Stories;

        }

        public List<City> GetCitiesForCountry(long countryid)
        {
            var cities = _db.Cities
                .Where(c => c.CountryId == countryid)
                .ToList();

            return cities;
        }
    }
}
